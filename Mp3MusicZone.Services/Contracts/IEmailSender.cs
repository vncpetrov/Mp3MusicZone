namespace Mp3MusicZone.Services.Contracts
{
	using System;
	using System.Threading.Tasks;

	public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
