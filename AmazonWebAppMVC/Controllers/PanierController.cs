using AmazonWebAppMVC.Data;
using AmazonWebAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AmazonWebAppMVC.Controllers
{
    public class PanierController : Controller
    {
        private readonly AmazonWebAppMVCContext _context;
        private const string SessionKeyName = ".Order.Session";
        
        public PanierController(AmazonWebAppMVCContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("/Panier")]

        public IActionResult Index()
        {


            //get cart from session or create new cart using serialization
            var item = HttpContext.Session.GetString(SessionKeyName);
            Order order = item == null ? new Order() : JsonSerializer.Deserialize<Order>(item);
            // get the products from the database
            foreach (OrderItem orderItem in order.OrderItems)
            {
                orderItem.Product = _context.Product.Find(orderItem.ProductId);
            }

            return View("Index",order);
        }

        // create a method to add a product to the cart
        public IActionResult AddToCart(int productId)
        {
           

            //get cart from session or create new cart using serialization
            var item = HttpContext.Session.GetString(SessionKeyName);
            Order order = item == null ? new Order() : JsonSerializer.Deserialize<Order>(item);
            //product price
            var product = _context.Product.Find(productId);
            //add product to cart
            order.AddItem(productId, product.Price);
            //save cart to session
            HttpContext.Session.SetString(SessionKeyName, JsonSerializer.Serialize(order));

            //redirect to cart page
            return RedirectToAction("Index");


        }
    }
}
