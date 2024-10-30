using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserRegistration_Backend.Models;

namespace UserRegistration_Backend.Data
{
    public class UserRegistration_BackendContext : DbContext
    {
        public UserRegistration_BackendContext (DbContextOptions<UserRegistration_BackendContext> options)
            : base(options)
        {
        }

        public DbSet<UserRegistration_Backend.Models.User> User { get; set; } = default!;
    }
}
