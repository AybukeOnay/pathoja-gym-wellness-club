using BusinessLayer.DTOs.Branch;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IBranchService : IGenericService<Branch>
    {
        Task<List<BranchListDto>> GetActiveBranchesAsync();
    }
}
