using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CapstoneMasons.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    string GMailAccount = _emailConfig.UserName;
                    var clientSecrets = new ClientSecrets  //using oath to send emails
                    {
                        ClientId = "877331268880-bp7c9d17r8noh385050ltedi6o95pvuu.apps.googleusercontent.com",
                        ClientSecret = "UzexzqVSI_1UKpLkZoOLkdqy"
                    };
                    var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                    {
                        // Cache tokens in ~/CredentialCacheFolder
                        DataStore = new FileDataStore("~/CredentialCacheFolder", true),
                        Scopes = new[] { "https://mail.google.com/" },
                        ClientSecrets = clientSecrets
                    });

                    var codeReceiver = new LocalServerCodeReceiver();
                    var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);
                    var credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

                    if (authCode.ShouldRequestAuthorizationCode(credential.Token))
                        await credential.RefreshTokenAsync(CancellationToken.None);
                    var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

                    client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    await client.AuthenticateAsync(oauth2);
                    
                    //client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

    }
}
