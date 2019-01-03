using AutoMapper;
using NetCoreAuthReact.Data.Models;
using NetCoreAuthReact.Dtos;

namespace NetCoreAuthReact.Contract.MappingProfiles
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
