using Tagerly.Services.Interfaces;
using System.Threading.Tasks;
using System;
using MimeKit;
using MailKit.Net.Smtp;

namespace Tagerly.Services.Implementations
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(string toEmail, string subject, string body)
		{
			var emailSettings = _configuration.GetSection("EmailSettings");
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
			message.To.Add(new MailboxAddress("", toEmail));
			message.Subject = subject;

			message.Body = new TextPart("html")
			{
				Text = body
			};

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
				await client.AuthenticateAsync(emailSettings["SmtpUsername"], emailSettings["SmtpPassword"]);
				await client.SendAsync(message);
				await client.DisconnectAsync(true);
			}
		}
		public string GetEmailConfirmationTemplate(string userName, string confirmationLink)
		{
			return $@"
                <h1>Hello {userName}!</h1>
                <p>Thank you for signing up with Tagerly. Click the button below to confirm your email:</p>
                <a href='{confirmationLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Confirm Account</a>
                <p>If you did not sign up, please ignore this email.</p>";
		}
	}
}