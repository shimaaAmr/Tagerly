using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tagerly.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
