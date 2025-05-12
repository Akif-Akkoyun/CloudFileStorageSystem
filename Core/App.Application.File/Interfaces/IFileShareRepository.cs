using App.Application.File.Dtos;
using App.Domain.Entities;

namespace App.Application.Interfaces.File
{
    public interface IFileShareRepository
    {
        Task CreateAsync(FileShareEntity entity);
        Task DeleteByFileIdAsync(int fileId);
        Task<List<FileEntity>> GetFilesSharedWithUserAsync(int userId);
        Task CreateRangeAsync(IEnumerable<FileShareEntity> entities);
        Task<List<SharedWithMeDto>> GetSharedWithMeAsync(int userId);
        Task<List<FileShareEntity>> GetByFileIdAsync(int fileId);
    }
}
