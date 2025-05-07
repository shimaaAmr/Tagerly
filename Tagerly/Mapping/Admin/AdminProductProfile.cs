using AutoMapper;
using Tagerly.Models;
using Tagerly.Models.Enums;
using Tagerly.Repositories.Interfaces;
using Tagerly.ViewModels;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Mapping.Admin
{
    public class AdminProductProfile : AutoMapper.Profile
    {

        private readonly IGenericRepo<Product> _repo;
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;

        public AdminProductProfile(IGenericRepo<Product> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public AdminProductProfile()
        {
            CreateMap<Product, ProductApprovingVM>()
                        .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller.UserName));

        }
        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _repo.GetAllAsync();
            var filtered = products.Where(p => p.Status != ProductStatus.Deleted);
            return _mapper.Map<IEnumerable<ProductViewModel>>(filtered);
        }



    }
}
