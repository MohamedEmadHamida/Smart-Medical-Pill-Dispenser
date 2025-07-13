using HealthCare.Project.Core.Dtos;
using HealthCare.Project.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Service.Services.Emails
{
    public class EmailService : IEmailService
    {
        public void SendEmail(EmailDto email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("healthcareteam7@gmail.com", "fpjbjifingyxoief")
            };
            string emailBody = $@"
              <html>
              <head>
                  <style>
                      .button {{
                          background-color: #007bff;
                          border: none;
                          color: white; /* Button text color */
                          padding: 12px 24px;
                          text-align: center;
                          text-decoration: none;
                          display: inline-block;
                          font-size: 16px;
                          margin: 10px 2px;
                          cursor: pointer;
                          border-radius: 5px;
                      }}
                  </style>
              </head>
              <body>
                  <p>You requested a password reset. Click the button below to proceed:</p>
                  <a href='{email.Body}' class='button' style='color: white;'>Reset Password</a>
              </body>
             </html>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("healthcareteam7@gmail.com"),
                Subject = email.Subject,
                Body = emailBody,
                IsBodyHtml = true // Enable HTML formatting
            };

            mailMessage.To.Add(email.To);
            client.Send(mailMessage);
        }

    }
}
