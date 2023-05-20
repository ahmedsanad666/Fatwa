using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebOS.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            StringBuilder header = new StringBuilder();

            header.Append("<center>");
            header.Append("<br/>");

            header.Append("<br/>");
            header.Append("</center>");

            StringBuilder trailer = new StringBuilder();

            trailer.Append("<center>");

            trailer.Append("<br/>");


            return emailSender.SendEmailAsync(email, "Confirm your Email at WebOS - تأكيد بريدك الإلكتروني", header +
                   $"<center><b><a href='{HtmlEncoder.Default.Encode(link)}'>Activation Link | رابط التفعيل </a> </p></b></center> <br/>" + trailer
             );
        }
    }
}
