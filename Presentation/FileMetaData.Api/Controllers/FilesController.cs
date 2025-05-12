using App.Application.Dtos.FileDtos;
using App.Application.Features.File.Commands;
using App.Application.Features.File.Queries;
using App.Application.File.Dtos;
using App.Application.File.Features.File.Commands;
using App.Application.File.Features.File.Queries;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileMetaData.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int ownerId)
        {
            var result = await _mediator.Send(new GetAllFileMetaDataListQuery(ownerId));
            return Ok(result);
        }
        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetFileMetaDataByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] FileCreateDto dto)
        {
            var fileId = await _mediator.Send(new CreateFileMetaDataCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = fileId }, new { id = fileId });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] FileUpdateDto dto)
        {
            var result = await _mediator.Send(new UpdateFileMetaDataCommand(dto));
            return result ? Ok("File updated.") : NotFound("File not found.");
        }
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _mediator.Send(new RemoveFileMetaDataCommand(id));
            return result ? Ok("File deleted.") : NotFound("File not found.");
        }
        [HttpGet("filters/public")]
        public async Task<IActionResult> FilePublicFilter()
        {
            var result = await _mediator.Send(new GetPublicFilesQuery());
            return Ok(result);
        }
        [HttpGet("shared-with-me/{userId}")]
        public async Task<IActionResult> GetSharedWithMe([FromRoute] int userId)
        {
            var result = await _mediator.Send(new GetSharedWithMeQuery(userId));
            return Ok(result);
        }
        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwnerId(int ownerId)
        {
            var result = await _mediator.Send(new GetFilesByOwnerIdQuery(ownerId));
            return Ok(result);
        }
        [HttpPut("toggle-visibility/{id}")]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            var result = await _mediator.Send(new ToggleFileVisibilityCommand(id));
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return NoContent();
        }
        [HttpPost("share-file")]
        public async Task<IActionResult> ShareFile([FromBody] ShareFileDto dto)
        {
            var result = await _mediator.Send(new ShareFileCommand(dto));

            if (result.IsSuccess)
                return Ok("File shared successfully.");

            if (result.Status == ResultStatus.NotFound)
                return NotFound("File not found.");

            return BadRequest(result.Errors);
        }
        [HttpGet("get-shares-by-file/{fileId}")]
        public async Task<IActionResult> GetSharesByFileId(int fileId)
        {
            var result = await _mediator.Send(new GetFileSharesByFileIdQuery(fileId));
            return Ok(result);
        }
    }
}