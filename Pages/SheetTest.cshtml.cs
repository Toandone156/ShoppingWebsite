using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoppingWebsite.Pages
{
    public class SheetTestModel : PageModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public int Total { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {

        }
    }
}
