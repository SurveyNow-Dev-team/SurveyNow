using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime DateOfBirth {  get; set; }
        public string AvatarUrl { get; set; }
    }
}
