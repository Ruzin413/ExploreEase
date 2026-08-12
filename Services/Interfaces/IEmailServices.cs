using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IEmailServices
    {
        Task<bool> DomainHasMxRecordAsync(string email);
        bool SendEmail(string toEmail, string subject, string body);
    }
}
