namespace Mp3MusicZone.Services
{
	using Contracts;
	using Microsoft.Extensions.Options;
	using Models;
	using System;
	using System.Net;
	using System.Net.Mail;
	using System.Threading.Tasks;

	public class EmailSender : IEmailSender
	{
		private readonly EmailSettings emailSettings;

		public EmailSender(IOptions<EmailSettings> emailSettings)
		{
			this.emailSettings = emailSettings.Value;
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			MailMessage mail = new MailMessage();
			
			mail.From = new MailAddress(this.emailSettings.UsernameEmail);
			mail.To.Add(new MailAddress(email));
			mail.Subject = subject;
			mail.Body = message;
			mail.IsBodyHtml = true;
			mail.Priority = MailPriority.High;

			using (SmtpClient smtpClient = 
				new SmtpClient(this.emailSettings.Domain, this.emailSettings.Port))
			{
				smtpClient.EnableSsl = true;
				smtpClient.Credentials = new NetworkCredential(
					this.emailSettings.UsernameEmail, this.emailSettings.UsernamePassword);

				await smtpClient.SendMailAsync(mail);
			}

			return;
		}
	}
}
