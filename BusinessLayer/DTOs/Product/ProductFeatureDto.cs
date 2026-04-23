using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Product
{
    public class ProductFeatureDto
    {
        public int Id { get; set; }
        public int? ProductHours { get; set; }
        public int? TotalLessonCount { get; set; }
        public bool IsCancellable { get; set; }
    }
}
