using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentitySystem.Models
{
    public class IdentitySystemContext : IdentityDbContext<AppUser>
    {
        public IdentitySystemContext (DbContextOptions<IdentitySystemContext> options)
            : base(options)
        {
        }

        public DbSet<IdentitySystem.Models.Course> Course { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

    }
}
