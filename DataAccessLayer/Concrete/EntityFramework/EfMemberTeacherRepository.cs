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
    public class EfMemberTeacherRepository : GenericRepository<MemberTeacher>, IMemberTeacherDal
    {
        public EfMemberTeacherRepository(AppDbContext context) : base(context)
        {
        }
    }
}
