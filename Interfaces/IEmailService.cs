using NOLA_API.Infrastructure.Messages;

namespace NOLA_API.Interfaces
{
    public interface IEmailService
    {
        public Task SendAsync(string email, Message message );
    }
}
