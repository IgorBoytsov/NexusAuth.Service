using AutoMapper;
using NexusAuth.Application.Features.Users;
using NexusAuth.Domain.Models;

namespace NexusAuth.Application.Mappers.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Конвертация ValueObject через implicit и переопределенный ToString
            CreateMap<User, UserDto>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                 .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login.Value))
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.Value))

                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                 .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone != null ? src.Phone.ToString() : null))

                 .ForMember(dest => dest.DateRegistration, opt => opt.MapFrom(src => src.DateRegistration))

                 .ForMember(dest => dest.IdStatus, opt => opt.MapFrom(src => src.IdStatus))
                 .ForMember(dest => dest.IdRole, opt => opt.MapFrom(src => src.IdRole))
                 .ForMember(dest => dest.IdGender, opt => opt.MapFrom(src => src.IdGender))
                 .ForMember(dest => dest.IdCountry, opt => opt.MapFrom(src => src.IdCountry));
        }
    }
}