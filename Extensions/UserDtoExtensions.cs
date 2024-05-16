
using NOLA_API.Domain;
using NOLA_API.DTOs;

namespace NOLA_API.Extensions
{
    public static class UserDtoExtensions
    {
        public static AppUser ToAppUser(this UserDto userDto)
        {
            return new AppUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Links = userDto.Links,
                Image = userDto.Image
            };
        }
    }
}