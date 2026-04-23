using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Payment
    {
        public int Id { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public DateTime PaymentDate { get; set; }
        public int Price { get; set; }
    }
}
