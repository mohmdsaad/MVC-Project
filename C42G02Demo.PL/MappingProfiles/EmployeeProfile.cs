using AutoMapper;
using C42G02Demo.DAL.Model;
using C42G02Demo.PL.ViewModels;

namespace C42G02Demo.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
                                                                   // Destination Mmeber  , Option => Option. --- 
            CreateMap<EmployeeViewModel, Employee>().ReverseMap(); //.ForMember(d=>d.Name , o => o.MapFrom(e => e.Name)) ;
        }
    }
}
