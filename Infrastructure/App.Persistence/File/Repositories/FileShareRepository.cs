using App.Application.File.Dtos;
using App.Application.Interfaces.File;
using App.Domain.Entities;
using App.Persistence.File.Context;
using Microsoft.EntityFrameworkCore;
namespace App.Persistence.File.Repositories
{
    internal class FileShareRepository : IFileShareRepository
    {
        private readonly FileMetaDataDbContext _context;

        public FileShareRepository(FileMetaDataDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(FileShareEntity entity)
        {
            _context.FileShares.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteByFileIdAsync(int fileId)
        {
            var shares = await _context.FileShares
                .Where(fs => fs.FileId == fileId)
                .ToListAsync();

            if (shares.Any())
            {
                _context.FileShares.RemoveRange(shares);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<FileEntity>> GetFilesSharedWithUserAsync(int userId)
        {
            return await _context.FileShares
                .Include(fs => fs.File)
                .Where(fs => fs.UserId == userId)
                .Select(fs => fs.File)
                .ToListAsync();
        }
        public async Task CreateRangeAsync(IEnumerable<FileShareEntity> entities)
        {
            await _context.FileShares.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        public async Task<List<SharedWithMeDto>> GetSharedWithMeAsync(int userId)
        {
            return await _context.FileShares
                .Include(fs => fs.File)
                .Where(fs => fs.UserId == userId)
                .Select(fs => new SharedWithMeDto
                {
                    Id = fs.File.Id,
                    FileName = fs.File.FileName,
                    Description = fs.File.Description,
                    FilePath = fs.File.FilePath,
                    UploadDate = fs.File.UploadDate,
                    Visibility = fs.File.Visibility,
                    OwnerId = fs.File.OwnerId,
                    Permission = fs.Permission
                })
                .ToListAsync();
        }
        public async Task<List<FileShareEntity>> GetByFileIdAsync(int fileId)
        {
            return await _context.FileShares
                .Where(fs => fs.FileId == fileId)
                .ToListAsync();
        }

    }
}