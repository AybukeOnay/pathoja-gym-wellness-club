using BusinessLayer.DTOs.User;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task AddUserAsync(SignUpDto dto);
        Task<bool> CheckUserAsync(string email, string password);
    }
}
