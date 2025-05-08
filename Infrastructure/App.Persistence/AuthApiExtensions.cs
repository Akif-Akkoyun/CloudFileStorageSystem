using App.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Persistence
{
    public static class AuthApiExtensions
    {
        public static void AddAuthApi(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<DbContext,AuthDbContext>(options =>
                options.UseSqlite(connectionString));
        }
    }
}
