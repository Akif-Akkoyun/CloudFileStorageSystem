using App.Application.Interfaces.Auth;
using App.Domain.Entities;
using App.Persistence.Auth.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Persistence.Auth.Repositories
{
    internal class UserRepository(AuthDbContext _context): IUserRepository
    {
        public async Task<List<UserEntity>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEntity?> GetByFilterAsync(Expression<Func<UserEntity, bool>> filter)
        {
            return await _context.Users.SingleOrDefaultAsync(filter);
        }

        public async Task<UserEntity> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id) ?? throw new InvalidCastException("not found");
        }

        public async Task CreateAsync(UserEntity entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserEntity entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(UserEntity entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}