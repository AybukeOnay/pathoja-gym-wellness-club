using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        public string Type { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<Payment> Payments { get; set; }
    }
}
