using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Product
{
    public class ProductInfoDto
    {
        public int Id { get; set; }               // Product.Id
        public int CategoryId { get; set; }       // Hangi hizmet / kategoriye ait

        public string Name { get; set; }          // Product_L.Name
        public string? Header { get; set; }       // Özel Ders / Grup Dersi 3'lü vb.
        public string? Description { get; set; }  // Kısa açıklama

        public List<ProductFeatureDto> Features { get; set; } = new();
        public List<ProductInfoDto> SubProducts { get; set; } = new();
    }
}
