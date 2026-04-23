using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class User
    {
        public int Id { get; set; }

        public int UserRoleId { get; set; }
        public UserRole Role { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; }

        [Required, MinLength(3), MaxLength(100)]
        public string LastName { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Mail { get; set; }

        [Required, MinLength(5),MaxLength(50)]
        public string Password { get; set; }

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; }
        public DateTime UserCreatedDate { get; set; }
        public bool Active { get; set; } = true;

        public Member? Member { get; set; } 
        public Teacher? Teacher { get; set; }
    }
}
