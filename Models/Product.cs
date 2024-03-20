using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingWebsite.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public int UnitPrice { get; set; }
        public string? ProductImage { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsDelete { get; set; } = false;

        //Relations
        [ForeignKey(nameof(CategoryID))]
        public virtual Category? Category { get; set; }
    }
}
