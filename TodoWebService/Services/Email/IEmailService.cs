using TodoWebService.Models.DTOs.Email;

namespace TodoWebService.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
    }
}
