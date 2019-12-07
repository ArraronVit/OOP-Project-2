using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace CProjectMapIos
{
    public  class EmailSender
    {
        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    // Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                Debug.WriteLine(fbsEx);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}