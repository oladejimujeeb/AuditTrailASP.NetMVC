using AuditTrailMVC.Models;
using AuditTrailMVC.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace AuditTrailMVC.Data
{
    public abstract class AuditableIdentityContext:IdentityDbContext
    {
        public AuditableIdentityContext(DbContextOptions options):base(options) 
        {

        }
        public DbSet<AuditTrail> AuditTrails { get; set; }  
        public virtual async Task<int> SaveChangesAsync( string userId =null)
        {
            OnSavingChanges(userId);
            return await base.SaveChangesAsync();
        }

        private void OnSavingChanges( string userId)
        {
            ChangeTracker.DetectChanges();
            List<AuditEntry> entries = new ();
            foreach(var entry in ChangeTracker.Entries())
            {
                if(entry.Entity is AuditTrail || entry.State ==EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }
                var auditEntry = new AuditEntry(entry);
                auditEntry.UserId = userId; 
                auditEntry.TableName = entry.Entity.GetType().Name;
                entries.Add(auditEntry);    
                foreach(var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch(entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Modified:
                            
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                    }
                }
            }
            foreach (var auditEntry in entries)
            {
                AuditTrails.Add(auditEntry.ToAudit());
            }
        }
    }
}
