using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Utils
{
    public static class SendEmails
    {
        public static async Task<bool> SendConfirmationEmail(
            string toEmail, string confirmationLink)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var username = config["SendEmails:UserName"];
            var emailfrom = config["SendEmails:EmailFrom"];
            var password = config["SendEmails:Password"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(username, emailfrom));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Confirmation Email";
            message.Body = new TextPart("html")
            {
                Text = @"
        <html>
            <head>
                <style>
                    body {
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                        margin: 0;
                        font-family: Arial, sans-serif;
                    }
                    .content {
                        text-align: center;
                    }
                    .button {
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #000;
                        color: #ffffff;
                        text-decoration: none;
                        border-radius: 5px;
                        font-size: 16px;
                    }
                </style>
            </head>
            <body>
                <div class='content'>
                    <p>Please click the button below to confirm your email:</p>
                    <a class='button' href='"
                    + confirmationLink
                    + "'>Confirm Email</a>"
                    + @"
                </div>
            </body>
        </html>
    "

            };

            using(var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //Authenticate Gmail Account
                client.Authenticate(emailfrom, password);
                try
                {
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return true;



                }catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    return false;
                }
            }


        }
    }
}
