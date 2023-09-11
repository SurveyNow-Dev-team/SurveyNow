using Application.DTOs.Request;
using Application.DTOs.Request.User;
using Application.DTOs.Response;
using Application.DTOs.Response.User;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>();
        CreateMap<UserResponse, UserRequest>().ReverseMap();
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.PasswordHash,
                src => src.MapFrom(src => BCrypt.Net.BCrypt.EnhancedHashPassword(src.Password)))
            .ForMember(dest => dest.FullName, src => src.MapFrom(src => StringUtil.GetNameFromEmail(src.Email)))
            .ForMember(dest => dest.Role, src => src.MapFrom(src => Role.User));
        CreateMap<User, LoginUserResponse>();
        
        
        CreateMap<Hobby, HobbyResponse>();
        CreateMap<HobbyRequest, Hobby>();

        CreateMap<Address, AddressResponse>();
        CreateMap<Province, ProvinceResponse>();
        CreateMap<City, CityResponse>();
        CreateMap<District, DistrictResponse>();
    }
}