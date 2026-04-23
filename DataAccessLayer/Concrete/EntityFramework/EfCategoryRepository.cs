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
    public class EfCategoryRepository : GenericRepository<Category>, ICategoryDal
    {
        public EfCategoryRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<Category>> GetActiveCategoriesForNavbarAsync()
        {
            return await _context.Category
                .Where(c => c.Active)
                .Include(c => c.Category_L)
                    .ThenInclude(cl => cl.Language)
                .Include(c => c.SubCategories.Where(sc => sc.Active))
                    .ThenInclude(sc => sc.Category_L)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesWithLanguagesAsync()
        {
            return await _context.Category
               .Include(c => c.Category_L)
                .ThenInclude(cl => cl.Language)
               .ToListAsync();
        }

        public IQueryable<Category> QueryForCategoryCards()
        {
            return _context.Category
                   .AsNoTracking()
                   .Where(c => c.Active)
                   .Include(c => c.Category_L)        
                   .Include(c => c.CategoryBranches)
                    .ThenInclude(cb => cb.Branch);
        }
    }
}
