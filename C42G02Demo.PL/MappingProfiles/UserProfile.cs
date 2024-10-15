using AutoMapper;
using C42G02Demo.DAL.Model;
using C42G02Demo.PL.ViewModels;

namespace C42G02Demo.PL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser , UserViewModel>().ReverseMap();
        }
    }
}
