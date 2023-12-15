using AmazonWebAppMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AmazonWebAppMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AmazonWebAppMVCContext _context;
        public HomeController(ILogger<HomeController> logger, AmazonWebAppMVCContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string? SearchTerm,int? CategoryId)
        {
            _logger.LogInformation("HomeController::Index({searchTerm},{categoryId}) called",SearchTerm,CategoryId);
            if(_context.Product == null || _context.Category == null)
            {
                _logger.LogError("HomeController::Index() _context.Product or _context.Category is null");
                return NotFound();
            }
            
            var products = from p in _context.Product.Include(p => p.Categorie) select p;
            
            if(!string.IsNullOrEmpty(SearchTerm))
            {
                products = products.Where(p => p.Name.Contains(SearchTerm));
            }

            if(CategoryId != null)
            {
                _logger.LogInformation("HomeController::Index() CategoryId = " + CategoryId);
                products = products.Where(p => p.CategoryId == CategoryId);
            }
            
            
            var categories = await _context.Category.ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
            _logger.LogInformation("HomeController::Index() products number = " + products.Count());
            return View(await products.ToListAsync());
        }

    
    }
}