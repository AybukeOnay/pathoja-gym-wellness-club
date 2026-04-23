using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.Branch;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class BranchManager : GenericManager<Branch>, IBranchService
    {
        private readonly IBranchDal _branchDal;
        private readonly IMapper _mapper;

        public BranchManager(IBranchDal branchDal, IMapper mapper) : base(branchDal)
        {
            _branchDal  = branchDal;
            _mapper = mapper;
        }

        public async Task<List<BranchListDto>> GetActiveBranchesAsync()
        {
            var entities = await _branchDal.GetActiveBranchAsync();
            return _mapper.Map<List<BranchListDto>>(entities);
        }
    }
}
