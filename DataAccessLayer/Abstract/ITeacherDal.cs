using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ITeacherDal : IGenericDal<Teacher>
    {
        Task<List<Teacher>> GetTeachersWithUserAndCategoryAsync(Expression<Func<Teacher, bool>> filter);
        Task AddTeacherWithRelationsAsync(User user,Teacher teacher,List<int> categoryIds,List<int> branchIds);
        Task<Teacher> GetTeacherWithRelationsAsync(int id);
        Task<List<Teacher>> GetTeachersWithUserBranchAndSkillsAsync(Expression<Func<Teacher, bool>> filter);
    }
}
