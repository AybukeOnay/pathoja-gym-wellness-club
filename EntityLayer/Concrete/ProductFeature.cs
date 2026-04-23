using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class ProductFeature
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int? ProductHours { get; set; }
        public int? TotalLessonCount { get; set; }
        public bool? IsCancellable { get; set; }       // iptalli/iptalsiz
        public bool Active { get; set; } = true;
    }
}
