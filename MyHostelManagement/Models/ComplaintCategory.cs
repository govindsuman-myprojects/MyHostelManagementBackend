using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class ComplaintCategory : BaseEntity
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int Status { get; set; }

        // Navigation
        public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
    }

}
