using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Contact
{
    public class ContactCreateDto
    {
        public ContactType Type { get; set; }

        public int CategoryId { get; set; }
        public int? ProductId { get; set; }
        public string? CategoryName { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string? Subject { get; set; }
        public string? Message { get; set; }

        public bool KvkkApproved { get; set; }
    }
}
