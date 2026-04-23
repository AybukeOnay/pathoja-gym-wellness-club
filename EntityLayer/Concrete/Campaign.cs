using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Campaign
    {
        public int Id { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public DateTime CampaignStartTime { get; set; }
        public DateTime? CampaignEndTime { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<ProductPrice> ProductPrices { get; set; }
        public ICollection<Campaign_L> Campaign_L { get; set; }
    }
}
