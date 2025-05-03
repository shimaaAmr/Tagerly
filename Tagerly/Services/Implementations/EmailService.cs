using Tagerly.Services.Interfaces;
using System.Threading.Tasks;
using System;

namespace Tagerly.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            // هذا تنفيذ تجريبي - استبدله بخدمة البريد الفعلية
            Console.WriteLine("=== Sending Email ===");
            Console.WriteLine($"To: {toEmail}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Content:\n{htmlContent}");

            await Task.Delay(500); // محاكاة عملية إرسال البريد
        }
    }
}