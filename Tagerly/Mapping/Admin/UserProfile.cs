using Tagerly.Models;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Mapping.Admin
{
    public class UserProfile:AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.Role, opt => opt.Ignore()) // هيتجاب من اليوزر مانيجر
                .ForMember(dest => dest.IsActive, opt =>
                    opt.MapFrom(src =>
                src.LockoutEnd == null || src.LockoutEnd <= DateTime.Now));
        }
    }
}
