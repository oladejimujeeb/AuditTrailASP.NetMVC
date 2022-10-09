using AuditTrailMVC.Data;
using AuditTrailMVC.Models;
using System.Threading.Tasks;

namespace AuditTrailMVC.Repository
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAuditTrail(Audit audit)
        {
            await _context.Audits.AddAsync(audit);
            return  audit.AuditId;
            //return await _context.SaveChangesAsync();
        }
    }
}
