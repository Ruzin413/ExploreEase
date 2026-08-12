using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ExtendDate(int id, int numb);
        Task<bool> UpdateReview(int id);
        Task<bool> InsertIntoPayment(IFormCollection form, string username, string email);
    }
}
