using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Context;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfTeacherRepository : GenericRepository<Teacher>, ITeacherDal
    {
        public EfTeacherRepository(AppDbContext context) : base(context) { }

        public async Task AddTeacherWithRelationsAsync(User user, Teacher teacher, List<int> categoryIds, List<int> branchIds)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            teacher.UserId = user.Id;
            await _context.Teacher.AddAsync(teacher);
            await _context.SaveChangesAsync();

            foreach (var categoryId in categoryIds)
            {
                await _context.CategoryTeacher.AddAsync(new CategoryTeacher
                {
                    TeacherId = teacher.Id,
                    CategoryId = categoryId
                });
            }

            foreach (var branchId in branchIds)
            {
                await _context.TeacherBranch.AddAsync(new TeacherBranch
                {
                    TeacherId = teacher.Id,
                    BranchId = branchId
                });
            }

            var categoryBranches = await _context.CategoryBranch
                .Where(cb => categoryIds.Contains(cb.CategoryId) && branchIds.Contains(cb.BranchId))
                .ToListAsync();

            foreach (var cb in categoryBranches)
            {
                await _context.TeacherCategoryBranch.AddAsync(new TeacherCategoryBranch
                {
                    TeacherId = teacher.Id,
                    CategoryBranchId = cb.Id
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Teacher>> GetTeachersWithUserAndCategoryAsync(Expression<Func<Teacher, bool>> filter)
        {
            return await _context.Teacher
                .Include(t => t.User)
                .Include(t => t.TeacherBranches)
                    .ThenInclude(tb => tb.Branch)
                .Include(t => t.CategoryTeachers)
                    .ThenInclude(ct => ct.Category)
                    .ThenInclude(c => c.Category_L)
                .Where(filter)
                .ToListAsync();
        }

        public async Task<Teacher> GetTeacherWithRelationsAsync(int id)
        {
            return await _context.Teacher
                .Include(t => t.User)
                .Include(t => t.CategoryTeachers)
                    .ThenInclude(ct => ct.Category)
                        .ThenInclude(cl => cl.Category_L)
                .Include(t => t.TeacherBranches)
                    .ThenInclude(tb => tb.Branch)
                .FirstOrDefaultAsync(t => t.Id == id && t.Active);
        }

        public async Task<List<Teacher>> GetTeachersWithUserBranchAndSkillsAsync(Expression<Func<Teacher, bool>> filter)
        {
            return await _context.Teacher
                .Include(t => t.User)
                .Include(t => t.TeacherBranches)
                    .ThenInclude(tb => tb.Branch)
                .Include(t => t.TeacherSkills)
                    .ThenInclude(ts => ts.Skill)
                        .ThenInclude(s => s.Skill_L)
                .Where(filter)
                .ToListAsync();
        }
    }
}
