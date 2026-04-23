using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? ParentProductId { get; set; }
        public Product? ParentProduct { get; set; }
        public ICollection<Product> SubProducts { get; set; } = new List<Product>();

        public string? Image { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<Product_L> Product_L { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }    
        public ICollection<ProductFeature> ProductFeatures { get; set; }    
        
    }
}
