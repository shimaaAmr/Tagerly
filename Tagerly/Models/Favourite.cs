using System.ComponentModel.DataAnnotations.Schema;

namespace Tagerly.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.Now;

        public ApplicationUser User { get; set; }

        public Product Product { get; set; }
    }
}
