using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Context;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfProductRepository : GenericRepository<Product>, IProductDal
    {
        public EfProductRepository(AppDbContext context) : base(context){}

        public async Task<List<Product>> GetProductsForCategoryWithLangAsync(int categoryId, int languageId)
        {
            return await _context.Product
                .Where(p => p.CategoryId == categoryId
                         && p.Active
                         && p.ParentProductId == null)   // sadece parent ürünler
                .Include(p => p.Product_L.Where(l => l.LanguageId == languageId))
                .Include(p => p.ProductFeatures.Where(f => f.Active))
                .Include(p => p.SubProducts.Where(sp => sp.Active))
                    .ThenInclude(sp => sp.Product_L.Where(l => l.LanguageId == languageId))
                .Include(p => p.SubProducts.Where(sp => sp.Active))
                    .ThenInclude(sp => sp.ProductFeatures.Where(f => f.Active))
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
    }
}
