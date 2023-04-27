using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Configuration;

namespace ADSystem.Helpers
{
	public class Email
    {
        public string AccentsEmail = WebConfigurationManager.AppSettings["FromEmail"].ToString();
        public string FernandaEmail = WebConfigurationManager.AppSettings["FernandaEmail"].ToString();
        public string AnnaEmail = WebConfigurationManager.AppSettings["AnnaEmail"].ToString();
        public string ManagerEmail = WebConfigurationManager.AppSettings["ManagerEmail"].ToString();
        public string SendGridKey = WebConfigurationManager.AppSettings["SendGridKey"].ToString();
        public bool GetSendMail = Convert.ToBoolean(WebConfigurationManager.AppSettings["SendMail"].ToString());
        public bool GetSendCC = Convert.ToBoolean(WebConfigurationManager.AppSettings["SendCC"].ToString());

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public void SendMail(string to, string subject, string body)
        {
            if (GetSendMail)
            {
                var client = new SendGridClient(this.SendGridKey);
                var message = new SendGridMessage();
                message.From = new EmailAddress(AccentsEmail, "Accents Decoration");
                message.AddTo(new EmailAddress(to, to));
                message.Subject = subject;
                message.HtmlContent = body;

                client.SendEmailAsync(message);
            }
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="emails">The emails.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public void SendMail(List<string> emails, string subject, string body)
        {
            if (GetSendMail)
            {
                var client = new SendGridClient(this.SendGridKey);
                var message = new SendGridMessage();
                message.From = new EmailAddress(AccentsEmail, "Accents Decoration");

                foreach (var email in emails)
                {
                    message.AddTo(new EmailAddress(email, email));
                }

                message.Subject = subject;
                message.HtmlContent = body;

                client.SendEmailAsync(message);
            }
        }

        /// <summary>
        /// Sends the mail with cc.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public void SendMailWithCC(string to, string subject, string body)
        {
            if (GetSendMail)
            {
                var client = new SendGridClient(this.SendGridKey);
                var message = new SendGridMessage();
                message.From = new EmailAddress(AccentsEmail, "Accents Decoration");
                message.AddTo(new EmailAddress(to, to));
                message.Subject = subject;

                if (this.GetSendCC)
                {
                    List<EmailAddress> emails = new List<EmailAddress>();
                    emails.Add(new EmailAddress(FernandaEmail));
                    emails.Add(new EmailAddress(AnnaEmail));
                    emails.Add(new EmailAddress(ManagerEmail));
                    message.AddBccs(emails);
                }

                message.HtmlContent = body;

                client.SendEmailAsync(message);
            }
        }

        /// <summary>
        /// Sends the mail with attachment.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachmentName">Name of the attachment.</param>
        /// <param name="memoryStream">The memory stream.</param>
        public void SendMailWithAttachment(string to, string subject, string body, string attachmentName, MemoryStream memoryStream)
        {
            if (this.GetSendMail)
            {
                var client = new SendGridClient(this.SendGridKey);
                var message = new SendGridMessage();

                message.From = new EmailAddress(AccentsEmail, "Accents Decoration");
                message.AddTo(new EmailAddress(to, to));
                message.Subject = subject;

                if (this.GetSendCC)
                {
                    List<EmailAddress> emails = new List<EmailAddress>();
                    emails.Add(new EmailAddress(FernandaEmail));
                    emails.Add(new EmailAddress(AnnaEmail));
                    emails.Add(new EmailAddress(ManagerEmail));
                    message.AddBccs(emails);
                }

                message.HtmlContent = body;
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                var file = Convert.ToBase64String(bytes);
                message.AddAttachment(attachmentName, file, null, null, null);

                client.SendEmailAsync(message);
            }
        }

        /// <summary>
        /// Sends the internal mail with attachment.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachmentName">Name of the attachment.</param>
        /// <param name="memoryStream">The memory stream.</param>
        public void SendInternalMailWithAttachment(string subject, string body, string attachmentName, MemoryStream memoryStream)
        {
            if (this.GetSendMail)
            {
                var client = new SendGridClient(this.SendGridKey);
                var message = new SendGridMessage();

                message.From = new EmailAddress(AccentsEmail, "Accents Decoration");
                message.AddTo(new EmailAddress(FernandaEmail, FernandaEmail));
                message.Subject = subject;

                if (this.GetSendCC)
                {
                    List<EmailAddress> emails = new List<EmailAddress>();
                    emails.Add(new EmailAddress(AnnaEmail));
                    emails.Add(new EmailAddress(ManagerEmail));
                    message.AddBccs(emails);
                }

                message.HtmlContent = body;
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                var file = Convert.ToBase64String(bytes);
                message.AddAttachment(attachmentName, file, null, null, null);

                client.SendEmailAsync(message);
            }
        }

		/// <summary>
		/// Sends the mail with attachment without cc.
		/// </summary>
		/// <param name="to">To.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <param name="attachmentName">Name of the attachment.</param>
		/// <param name="memoryStream">The memory stream.</param>
		public void SendMailWithAttachmentWithoutCC(string to, string subject, string body, string attachmentName, MemoryStream memoryStream)
        {
            if (this.GetSendMail)
            {
                var client = new SendGridClient(this.SendGridKey);
                var message = new SendGridMessage();

                message.From = new EmailAddress(AccentsEmail, "Accents Decoration");
                message.AddTo(new EmailAddress(to, to));
                message.Subject = subject;

                message.HtmlContent = body;
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                var file = Convert.ToBase64String(bytes);
                message.AddAttachment(attachmentName, file, null, null, null);

                client.SendEmailAsync(message);
            }
        }
    }
}