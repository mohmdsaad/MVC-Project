using AutoMapper;
using C42G02Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace C42G02Demo.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole,RoleViewModel>()
                .ForMember(m=>m.RoleName , o => o.MapFrom(s=>s.Name))
                .ReverseMap();
        }
    }
}
