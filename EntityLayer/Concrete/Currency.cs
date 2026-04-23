using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }

    }
}
