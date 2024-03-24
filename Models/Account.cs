using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsite.Models
{
    public enum AccountType
    {
        Member = 0,
        Staff = 1
    }

    public class Account
    {
        [Key]
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public AccountType Type { get; set; }
        public long? TelegramID { get; set; }

        //Relations
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
