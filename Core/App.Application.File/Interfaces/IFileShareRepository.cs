using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces.File
{
    public interface IFileShareRepository
    {
        Task CreateAsync(FileShareEntity entity);
        Task DeleteByFileIdAsync(int fileId);
        Task<List<FileEntity>> GetFilesSharedWithUserAsync(int userId);
        Task<List<FileShareEntity>> GetUsersWithFileSharedAsync(int fileId);
    }
}
