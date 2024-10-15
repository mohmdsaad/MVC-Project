using C42G02Demo.DAL.Model;
using System.Net;
using System.Net.Mail;

namespace C42G02Demo.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("stmp.gmail.com",587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("mohamedsaadiio345@gmail.com", "sxuvcmyketquafsj");
			Client.Send("mohamedsaadiio345@gmail.com", email.To, email.Subject, email.Body);
		}

	}
}
