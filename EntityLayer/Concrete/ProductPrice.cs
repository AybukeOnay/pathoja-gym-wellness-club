using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class ProductPrice
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProductOptionId { get; set; }
        public ProductOption ProductOption { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int? CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public decimal Price { get; set; }
        public decimal NetPrice { get; set; }
        public DateTime PackagePriceStartDate { get; set; }
        public DateTime? PackagePriceEndDate { get; set; }
        public bool Active { get; set; } = true;

    }
}
