using BusinessLayer.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IProductService
    {
        Task<List<ProductInfoDto>> GetProductsForCategoryAsync(int categoryId,int languageId);
    }
}
