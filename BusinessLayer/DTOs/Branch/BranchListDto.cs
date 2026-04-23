using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Branch
{
    public class BranchListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? GoogleMapsUrl { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Code { get; set; }   //Category List için Kullandık
    }
}
