﻿using Application.DTOs.Request.User;
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
        CreateMap<UserRequest, User>().ForAllMembers(x => x.Condition((src, dest, sourceValue) => sourceValue != null));
        CreateMap<UserResponse, UserRequest>().ReverseMap();
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.PasswordHash,
                src => src.MapFrom(src => BCrypt.Net.BCrypt.EnhancedHashPassword(src.Password)))
            .ForMember(dest => dest.FullName, src => src.MapFrom(src => StringUtil.GetNameFromEmail(src.Email)))
            .ForMember(dest => dest.Role, src => src.MapFrom(src => Role.User));
        CreateMap<User, LoginUserResponse>();

        CreateMap<UserFilterRequest, User>().ForAllMembers(x => x.Condition((src, dest, sourceValue) => sourceValue != null));
        
        CreateMap<Role, string>().ConstructUsing(src => src.ToString());
        CreateMap<string, Role>().ConstructUsing(src => Enum.Parse<Role>(src));
        CreateMap<UserStatus, string>().ConstructUsing(src => src.ToString());
        CreateMap<string, UserStatus>().ConstructUsing(src => Enum.Parse<UserStatus>(src));
        CreateMap<Gender, string>().ConstructUsing(src => src.ToString());
        CreateMap<string, Gender>().ConstructUsing(src => Enum.Parse<Gender>(src));
        CreateMap<string, RelationshipStatus>().ConstructUsing(src => Enum.Parse<RelationshipStatus>(src));
        CreateMap<RelationshipStatus, string>().ConstructUsing(src => src.ToString());

        CreateMap<Hobby, HobbyResponse>();
        CreateMap<HobbyRequest, Hobby>();
        CreateMap<OccupationRequest, Occupation>()
            .AfterMap((src, dest) =>
            {
                if(src.Currency == null)
                {
                    dest.Currency = "VND";
                }
            })
            .ForAllMembers(x => x.Condition((src, dest, sourceValue) => sourceValue != null));
        CreateMap<Occupation, OccupationResponse>();
        CreateMap<Field, FieldDTO>().ReverseMap();

        CreateMap<AddressRequest, Address>();
        CreateMap<Address, AddressResponse>();
        CreateMap<Province, ProvinceResponse>().ReverseMap();
        CreateMap<City, CityResponse>().ReverseMap();
        CreateMap<District, DistrictResponse>().ReverseMap();

        CreateMap<UserReportRequest, UserReport>()
            .AfterMap((src, dest) =>
            {
                dest.Status = UserReportStatus.Pending;
            })
            .ForAllMembers(x => x.Condition((src, dest, sourceValue) => sourceValue != null));
        CreateMap<UserReport, UserReportResponse>();
        CreateMap<UserReportStatusRequest, UserReport>().ForAllMembers(x => x.Condition((src, dest, sourceValue) => sourceValue != null));
        CreateMap<string, UserReportStatus>().ConstructUsing(src => Enum.Parse<UserReportStatus>(src));
        CreateMap<UserReportStatus, string>().ConstructUsing(src => src.ToString());
    }
}