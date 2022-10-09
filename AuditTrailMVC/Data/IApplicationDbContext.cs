using AuditTrailMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuditTrailMVC.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Student> Student { get; set; }
        DbSet<Audit> Audits { get; set; }

        Task<int> SaveChangeAsync();
    }
}