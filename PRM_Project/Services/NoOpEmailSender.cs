using Microsoft.AspNetCore.Identity;
using PRM_Project.Models;

namespace PRM_Project.Services 
{
    public class OpEmailSender : IEmailSender<User>
    {
        public Task SendConfirmationLinkAsync(User user, string email, string confirmationUrl) =>
            Task.CompletedTask;

        public Task SendPasswordResetLinkAsync(User user, string email, string resetUrl) =>
            Task.CompletedTask;

        public Task SendEmailAsync(User user, string email, string subject, string htmlMessage) =>
            Task.CompletedTask;

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }
    }
}
