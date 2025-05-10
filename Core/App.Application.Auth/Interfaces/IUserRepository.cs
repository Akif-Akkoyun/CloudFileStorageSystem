using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces.Auth
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity> GetByIdAsync(int id);
        Task<UserEntity?> GetByFilterAsync(Expression<Func<UserEntity, bool>> filter);
        Task CreateAsync(UserEntity entity);
        Task UpdateAsync(UserEntity entity);
        Task RemoveAsync(UserEntity entity);
    }
}
