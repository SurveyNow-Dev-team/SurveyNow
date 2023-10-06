namespace Application.DTOs.Response.User
{
    public class HobbyResponse
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public UserResponse UserResponse { get; set; }
    }
}
