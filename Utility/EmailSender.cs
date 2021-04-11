using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCRUD.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient("98a53a554f8b8bdddcb037ef9780f136","22921ee92e0f7587254b0614804ee52a")
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "aakash.khadka@texasintl.edu.np"},
        {"Name", "Aakash"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "Aakash"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
             "Dear "+ email,
    
       "Your account has been created, check out."
      }, {
       "HTMLPart",
       htmlMessage
      
      
      }
     }
             });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
