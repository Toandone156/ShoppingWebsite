using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsite.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; } = false;

        //Relations
        public virtual ICollection<Product>? Products { get; set; }
    }
}
