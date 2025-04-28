using System.ComponentModel.DataAnnotations.Schema;

namespace Tagerly.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; }

        //Navigation property
        public ApplicationUser User { get; set; }
    }
}
