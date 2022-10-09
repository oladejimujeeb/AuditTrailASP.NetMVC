using System;

namespace AuditTrailMVC.Models
{
    public class Audit
    {
        public int AuditId { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string IpAddress { get; set; }
        public string IsFirstLogin { get; set; }
        public string VisitDate { get; set; } = DateTime.UtcNow.ToString("F");
        public string LoggedInAt { get; set; }
        public string LoggedOutAt { get; set; }
        public string LoginStatus { get; set; }
        public string PageAccessed { get; set; }
        public string SessionId { get; set; }
        public string UrlReferrer { get; set; }
        public string UserId { get; set; }
    }
}
