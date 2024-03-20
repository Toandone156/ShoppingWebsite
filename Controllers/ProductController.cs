using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using ShoppingWebsite.Services;

namespace ShoppingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private StoreContext _context;

        public ProductController(StoreContext context)
        {
            _context = context;
        }

        [Route("ChangeAvailable")]
        [HttpPost]
        public object ChangeAvailable([FromBody] object data)
        {
            dynamic deData = JsonConvert.DeserializeObject(data.ToString());

            var productid = (int)deData.id;

            var product  = _context.Products.Find(productid);

            if (product == null) return new
            {
                status = false
            };

            product.IsAvailable = !product.IsAvailable;

            _context.SaveChanges();

            return new
            {
                status = true,
                stockValue = product.IsAvailable
            };
        }
    }
}
