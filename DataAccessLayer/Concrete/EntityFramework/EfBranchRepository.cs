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
    public class EfBranchRepository : GenericRepository<Branch>, IBranchDal
    {
        public EfBranchRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Branch>> GetActiveBranchAsync()
        {
            return await _context.Branch
                .Where(x => x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
