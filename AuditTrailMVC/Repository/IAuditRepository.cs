using AuditTrailMVC.Models;
using System.Threading.Tasks;

namespace AuditTrailMVC.Repository
{
    public interface IAuditRepository
    {
        Task<int> AddAuditTrail(Audit audit);
    }
}
