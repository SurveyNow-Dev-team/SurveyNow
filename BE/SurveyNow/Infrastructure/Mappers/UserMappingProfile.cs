using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappers;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>();
        CreateMap<UserResponse, UserRequest>().ReverseMap();

        CreateMap<Hobby, HobbyResponse>();
        CreateMap<HobbyRequest, Hobby>();

        CreateMap<Address, AddressResponse>();
        CreateMap<Province, ProvinceResponse>();
        CreateMap<City, CityResponse>();
        CreateMap<District, DistrictResponse>();
    }
}