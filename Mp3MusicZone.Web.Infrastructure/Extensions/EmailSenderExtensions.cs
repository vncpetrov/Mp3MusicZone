namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
	using System;
	using System.Text.Encodings.Web;
	using System.Threading.Tasks;
	using Services.Contracts;

	public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSenderService emailSender, string email, string link)
        {
            return emailSender
				.SendEmailAsync(
				email,
				"Your registration is almost complete!",
                "<h2>Thanks for signing up for Mp3MusicZone!</h2>" +
				$"<h4>Please verify your account by clicking the link below: <a href='{HtmlEncoder.Default.Encode(link)}'>{HtmlEncoder.Default.Encode(link)}</a></h4>");
        }
    }
}
