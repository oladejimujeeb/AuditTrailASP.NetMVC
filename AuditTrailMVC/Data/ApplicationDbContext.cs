using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using AuditTrailMVC.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AuditTrailMVC.Data
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Student { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public async Task<int> SaveChangeAsync()
        {
            return await base.SaveChangesAsync();
        }

        internal Task SaveChangesAsync(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
    }
}
