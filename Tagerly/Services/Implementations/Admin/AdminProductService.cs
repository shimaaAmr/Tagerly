using AutoMapper;
using Tagerly.Models.Enums;
using Tagerly.Models;
using Tagerly.ViewModels.AdminViewModel;
using Tagerly.Repositories.Implementations;
using Tagerly.Repositories.Interfaces;
using Tagerly.Services.Interfaces.Admin;
using Tagerly.ViewModels;
using NuGet.Protocol.Core.Types;

namespace Tagerly.Services.Implementations.Admin
{
    public class AdminProductService : IAdminProductService
    {
        private readonly IGenericRepo<Product> _repo;
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;

        public AdminProductService(IGenericRepo<Product> repo,IProductRepo productRepo ,IMapper mapper)
        {
            _repo = repo;
            _productRepo= productRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductApprovingVM>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProductApprovingVM>>(products);
        }


        public async Task<bool> ChangeApprovalStatusAsync(int id, bool isApproved)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;

            product.Status = isApproved ? ProductStatus.Approved : ProductStatus.Rejected;
            _repo.Update(product);
            await _repo.SaveChangesAsync();
            return true;
        }


        public async Task<bool> RejectProductAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;
            product.Status = ProductStatus.Rejected;
            _repo.Update(product);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return false;

            product.Status = ProductStatus.Deleted;
            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync();

            return true;
        }

        public async Task<ProductApprovingVM> GetProductByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return null;

            return _mapper.Map<ProductApprovingVM>(product);
        }

    }
}
