namespace NOLA_API.DTOs
{
    public class UserDto
    {
        public string DisplayName { get; set; } //use only oine of them
        public string Token { get; set; }
        public string Image { get; set; } 
        public string Username { get; set; }
        public string Email { get; set; }
    }
}