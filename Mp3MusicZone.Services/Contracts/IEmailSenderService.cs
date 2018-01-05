namespace Mp3MusicZone.Services.Contracts
{
	using System;
	using System.Threading.Tasks;

	public interface IEmailSenderService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
