﻿using System.Threading.Tasks;

namespace Tagerly.Services.Interfaces
{
	public interface IEmailService
	{
		Task SendEmailAsync(string toEmail, string subject, string htmlContent);
		string GetEmailConfirmationTemplate(string userName, string confirmationLink);
		string GetPasswordResetTemplate(string userName, string resetLink);
	}
}
