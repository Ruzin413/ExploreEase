using DnsClient; 
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace Services.Services
{
    public class EmailServices
    {

        public async Task<bool> DomainHasMxRecordAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            return false;

        var domain = email.Split('@').Last();

        try
        {
            var lookup = new LookupClient();
            var result = await lookup.QueryAsync(domain, QueryType.MX);
            var mxRecords = result.Answers.MxRecords();

            return mxRecords.Any();
        }
        catch
        {
            return false;
        }
    }

    public bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var fromEmail = "exploreease413@gmail.com";
                var fromPassword = "fqwiailnxkedcsoj"; 

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    smtp.EnableSsl = true;

                    var message = new MailMessage(fromEmail, toEmail, subject, body)
                    {
                        IsBodyHtml = true 
                    };

                    smtp.Send(message);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
