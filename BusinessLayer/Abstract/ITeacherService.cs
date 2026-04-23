using BusinessLayer.DTOs.Teacher;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ITeacherService : IGenericService<Teacher>
    {
        Task<List<TeacherListDto>> GetActiveTeachersWithUserAndCategory();
        Task AddTeacherAsync(TeacherAddDto teacherAddDto);
        Task<TeacherUpdateDto> GetTeacherForUpdateAsync(int id);
        Task UpdateTeacherAsync(TeacherUpdateDto teacherUpdateDto);
        Task<List<TeacherListForWebsiteDto>> GetTeacherListForWebsiteAsync();
    }
}
