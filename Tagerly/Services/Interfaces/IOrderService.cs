using Tagerly.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tagerly.Models.Enums;

namespace Tagerly.Services.Interfaces
{

        public interface IOrderService
        {
        Task<Order> PlaceOrder(
            string userId,
            PaymentMethod paymentMethod,
            string shippingAddress,
            string email);

     
        }
    



    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlContent);
    }
}