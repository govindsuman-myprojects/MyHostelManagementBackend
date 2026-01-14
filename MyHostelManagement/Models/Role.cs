using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class Role : BaseEntity
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = string.Empty;

        // Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<TermsAndConditions> TermsAndConditions { get; set; } = new List<TermsAndConditions>();
    }

}
