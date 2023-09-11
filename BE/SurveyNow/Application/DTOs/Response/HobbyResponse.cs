using Application.DTOs.Response.User;

namespace Application.DTOs.Response
{
    public class HobbyResponse
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public UserResponse UserResponse { get; set; }
    }
}
