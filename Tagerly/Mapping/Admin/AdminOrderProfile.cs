using Tagerly.Models;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Mapping.Admin
{
    public class AdminOrderProfile: AutoMapper.Profile
    {
        public AdminOrderProfile()
        {
            CreateMap<Order, AdminOrderViewModel>()
           .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));

        }
    }
}
