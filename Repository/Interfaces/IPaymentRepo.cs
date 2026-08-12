using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IPaymentRepo
    {
        Task<bool> ExtendDate(int id, int numberOfDays);
        Task<bool> UpdateReview(int id);
        Task<bool> InsertIntoDB(PaymentModel model);
    }
}
