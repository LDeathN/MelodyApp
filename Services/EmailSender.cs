using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
namespace MelodyApp.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // For development: no real email sending.
            Console.WriteLine($"Sending email to {email}: {subject}");
            return Task.CompletedTask;
        }
    }
}
