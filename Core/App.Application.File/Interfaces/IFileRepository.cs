using App.Domain.Entities;
using System.Linq.Expressions;
namespace App.Application.Interfaces.File
{
    public interface IFileRepository
    {
        Task<int> CreateAsync(FileEntity file);
        Task<FileEntity?> GetByIdAsync(int id, bool includeShares = false);
        Task<List<FileEntity>> GetAllAsync(int ownerId);
        Task UpdateAsync(FileEntity file);
        Task DeleteAsync(FileEntity file);
        Task<List<FileEntity>> GetByFilterAsync(Expression<Func<FileEntity, bool>> person);
    }
}
