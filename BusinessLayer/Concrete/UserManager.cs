using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.User;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserManager : GenericManager<User>, IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;

        public UserManager(IUserDal userDal, IMapper mapper) : base(userDal)
        {
            _userDal = userDal;
            _mapper = mapper;
        }

        public async Task AddUserAsync(SignUpDto dto)
        {
            var existingUser = await _userDal.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Bu email adresiyle zaten bir kullanıcı mevcut.");

            var user = _mapper.Map<User>(dto);
            await _userDal.InsertAsync(user);
        }

        public async Task<bool> CheckUserAsync(string email, string password)
        {
            var user = await _userDal.GetByEmailAsync(email);

            if (user == null)
                return false;

            // şimdilik plain string karşılaştırma
            return user.Password == password;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userDal.GetByEmailAsync(email);
        }
    }
}
