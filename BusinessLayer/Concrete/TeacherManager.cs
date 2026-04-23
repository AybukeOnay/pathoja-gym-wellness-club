using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.Teacher;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class TeacherManager : GenericManager<Teacher>, ITeacherService
    {
        private readonly ITeacherDal _teacherDal;
        private readonly IMapper _mapper;

        public TeacherManager(ITeacherDal teacherDal, IMapper mapper) : base(teacherDal)
        {
            _teacherDal = teacherDal;
            _mapper = mapper;
        }

        public async Task AddTeacherAsync(TeacherAddDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Mail = dto.Mail,
                Password = dto.Password, // Hash önerilir
                PhoneNumber = dto.PhoneNumber,
                UserCreatedDate = DateTime.UtcNow,
                Active = true,
                UserRoleId = 3
               
            };

            var teacher = new Teacher
            {
                Image = dto.Image,
                Active = true
            };

            await _teacherDal.AddTeacherWithRelationsAsync(user, teacher,dto.CategoryIds,dto.BranchIds);
        }

        public async Task<List<TeacherListDto>> GetActiveTeachersWithUserAndCategory()
        {
            var teachers = await _teacherDal.GetTeachersWithUserAndCategoryAsync(t => t.Active && t.User.Active);
            return _mapper.Map<List<TeacherListDto>>(teachers);
        }

        public async Task<TeacherUpdateDto> GetTeacherForUpdateAsync(int id)
        {
            var teacher = await _teacherDal.GetTeacherWithRelationsAsync(id);
            if (teacher == null) return null;

            return _mapper.Map<TeacherUpdateDto>(teacher);
        }

       
        public async Task UpdateTeacherAsync(TeacherUpdateDto teacherUpdateDto)
        {
            var teacher = await _teacherDal.GetTeacherWithRelationsAsync(teacherUpdateDto.Id);

            //IMAGE
            teacher.User.Name = teacherUpdateDto.Name;
            teacher.User.LastName = teacherUpdateDto.LastName;
            teacher.User.Mail = teacherUpdateDto.Mail;
            teacher.User.PhoneNumber = teacherUpdateDto.PhoneNumber;

            teacher.CategoryTeachers.Clear();
            foreach(var catId in teacherUpdateDto.CategoryIds)
            {
                teacher.CategoryTeachers.Add(new CategoryTeacher
                {
                    TeacherId = teacher.Id,
                    CategoryId = catId
                });
            }

            teacher.TeacherBranches.Clear();
            foreach (var branchId in teacherUpdateDto.BranchIds)
            {
                teacher.TeacherBranches.Add(new TeacherBranch
                {
                    TeacherId = teacher.Id,
                    BranchId = branchId
                });
            }

            await _teacherDal.UpdateAsync(teacher);
        }


        //WEB SITE
        public async Task<List<TeacherListForWebsiteDto>> GetTeacherListForWebsiteAsync()
        {
            var teachers = await _teacherDal.GetTeachersWithUserBranchAndSkillsAsync(t => t.Active);

            var result = _mapper.Map<List<TeacherListForWebsiteDto>>(teachers);

            return result;
        }

    }
}
