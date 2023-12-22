using AmazonWebAppMVC.Data;
using AmazonWebAppMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AmazonWebAppMVC.Controllers
{
    [Authorize(Roles= "Client")]
    public class PanierController : Controller
    {
        private readonly AmazonWebAppMVCContext _context;
        private readonly ILogger _logger;
        private const string SessionKeyName = ".Order.Session";
        
        public PanierController(AmazonWebAppMVCContext context, ILogger<PanierController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult Index()
        {


            //get cart from session or create new cart using serialization
            _logger.LogInformation("PanierController::Index() called");
            var item = HttpContext.Session.GetString(SessionKeyName);
            _logger.LogInformation("PanierController::Index() item = " + item);
            Order order = item == null ? new Order() : JsonSerializer.Deserialize<Order>(item);
            // get the products from the database
            foreach (OrderItem orderItem in order.OrderItems)
            {
                orderItem.Product = _context.Product.Find(orderItem.ProductId);
            }
            _logger.LogInformation("PanierController::Index() order = " + order);
            return View("Index",order);
        }

        // create a method to add a product to the cart
        public IActionResult AddToCart(int productId)
        {
           

            //get cart from session or create new cart using serialization
            var item = HttpContext.Session.GetString(SessionKeyName);
            Order order = item == null ? new Order() : JsonSerializer.Deserialize<Order>(item);
            _logger.LogInformation("PanierController::AddToCart() Order before = " + order);
            //product price
            var product = _context.Product.Find(productId);
            _logger.LogInformation("PanierController::AddToCart() product = " + product);
            //add product to cart
            order.AddItem(productId, product.Price);
            //refresh total
            order.Total += (int)product.Price;
            //save cart to session
            _logger.LogInformation("PanierController::AddToCart() Order after = " + order);
            HttpContext.Session.SetString(SessionKeyName, JsonSerializer.Serialize(order));

            //redirect to cart page
            return RedirectToAction("Index");
        }
    }
}
