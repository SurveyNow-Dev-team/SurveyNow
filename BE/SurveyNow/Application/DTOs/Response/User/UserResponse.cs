﻿using Domain.Enums;

namespace Application.DTOs.Response.User
{
    public class UserResponse
    {
        public long? Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth {  get; set; }
        public string AvatarUrl { get; set; }
        public UserStatus Status { get; set; }
        public Role Role { get; set; }
        public RelationshipStatus? RelationshipStatus { get; set; }
        public string? LangKey { get; set; }
        public string? Currency { get; set; }
        public AddressResponse? Address { get; set; }
        public OccupationResponse? Occupation { get; set;}
    }
}
