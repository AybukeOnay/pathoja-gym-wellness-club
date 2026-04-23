using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.DTOs.Product;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ProductManager : GenericManager<Product>, IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IMapper _mapper;

        public ProductManager(IProductDal productDal, IMapper mapper) : base(productDal)
        {
            _productDal = productDal;
            _mapper = mapper;
        }

        public async Task<List<ProductInfoDto>> GetProductsForCategoryAsync(int categoryId, int languageId)
        {
            var products = await _productDal.GetProductsForCategoryWithLangAsync(categoryId, languageId); 
            var dtos = _mapper.Map<List<ProductInfoDto>>(products);
            return dtos;
        }
    }
}
