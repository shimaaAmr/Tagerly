using System.ComponentModel.DataAnnotations.Schema;

namespace Tagerly.Models
{
    public class Cart
    {
       public int Id { get; set; }
       
        //Navigation property
       [ForeignKey("ApplicationUser")]
       public string UserId { get; set; }

       public ICollection<Product> Products { get; set; }
       public ApplicationUser User { get; set; }



    }
}
