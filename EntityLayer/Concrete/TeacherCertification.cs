using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class TeacherCertification
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public string CertificationName { get; set; }
        public string? IssuingOrganization { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CertificationNumber { get; set; }
        //public CertificationLevel Level { get; set; }
        public bool Active { get; set; } = true;
    }
}
