namespace NOLA_API.DTOs
{
    public class UserDto
    {
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public string? Image { get; set; } 
        public string? Email { get; set; }
        public List<string>? Links { get; set; }
    }
}