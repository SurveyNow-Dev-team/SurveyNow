using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class HobbyResponse
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public UserResponse UserResponse { get; set; }
    }
}
