using AutoMapper;
using Tagerly.Models;
using Tagerly.ViewModels;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Mapping.Admin
{
    public class AdminProductProfile : AutoMapper.Profile
    {

        public AdminProductProfile()
        {
            CreateMap<Product, ProductApprovingVM>()
                        .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller.UserName));

        }


    }
}
