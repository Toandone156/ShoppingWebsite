using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingWebsite.Models
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }

        //Relations
        [ForeignKey(nameof(OrderID))]
        public virtual Order? Order { get; set; }
        [ForeignKey(nameof(ProductID))]
        public virtual Product? Product { get; set; }
    }
}
