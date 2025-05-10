using App.Application.Interfaces.File;
using App.Domain.Entities;
using App.Persistence.File.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Persistence.File.Repositories
{
    internal class FileRepository : IFileRepository
    {
        private readonly FileMetaDataDbContext _fileMetaDataDb;

        public FileRepository(FileMetaDataDbContext fileMetaDataDb)
        {
            _fileMetaDataDb = fileMetaDataDb;
        }
        public async Task<int> CreateAsync(FileEntity file)
        {
            _fileMetaDataDb.Files.Add(file);
            await _fileMetaDataDb.SaveChangesAsync();
            return file.Id;
        }
        public async Task<FileEntity?> GetByIdAsync(int id, bool includeShares = false)
        {
            if (includeShares)
            {
                return await _fileMetaDataDb.Files
                    .Include(f => f.FileShares)
                    .FirstOrDefaultAsync(f => f.Id == id);
            }
            return await _fileMetaDataDb.Files
                .FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task<List<FileEntity>> GetAllAsync(int ownerId)
        {
            return await _fileMetaDataDb.Files
                .Include(f => f.FileShares)
                .Where(f => f.OwnerId == ownerId)
                .ToListAsync();
        }
        public async Task UpdateAsync(FileEntity file)
        {
            _fileMetaDataDb.Files.Update(file);
            await _fileMetaDataDb.SaveChangesAsync();
        }
        public async Task DeleteAsync(FileEntity file)
        {
            _fileMetaDataDb.Files.Remove(file);
            await _fileMetaDataDb.SaveChangesAsync();
        }
        public async Task<List<FileEntity>> GetByFilterAsync(Expression<Func<FileEntity, bool>> person)
        {
            return await _fileMetaDataDb.Files
                .Include(f => f.FileShares)
                .Where(person)
                .ToListAsync();
        }
    }
}