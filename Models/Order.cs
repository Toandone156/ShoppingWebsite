using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingWebsite.Models
{
    public enum OrderStatus
    {
        Sent = 0,
        Processing = 1,
        Shipping = 2,
        Done = 3
    }

    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipAddress { get; set; }
        public string Phone { get; set; }
        public OrderStatus status { get; set; } = OrderStatus.Sent;

        //Foreign key
        public int AccountID { get; set; }

        //Relations
        [ForeignKey(nameof(AccountID))]
        public virtual Account? Account { get; set; }
        public virtual ICollection<OrderDetail>? Details { get; set; }

        [NotMapped]
        public int TotalAmount { get; set; } = 0;
    }
}
