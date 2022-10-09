using System;

namespace AuditTrailMVC.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }   
    }
}
