using System.ComponentModel.DataAnnotations;

namespace Tagerly.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }

        // ده مجرد string أو رقم وهمي يمثل رقم المعاملة
        public string TransactionId { get; set; }

        public int OrderId { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Order Order { get; set; }
    }
}
