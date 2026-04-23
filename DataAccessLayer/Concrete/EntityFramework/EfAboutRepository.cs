using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Context;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfAboutRepository : GenericRepository<About>, IAboutDal
    {
        public EfAboutRepository(AppDbContext context) : base(context)
        {
        }
    }
}
