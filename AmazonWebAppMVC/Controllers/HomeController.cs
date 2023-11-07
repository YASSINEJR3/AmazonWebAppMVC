using AmazonWebAppMVC.Data;
using AmazonWebAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AmazonWebAppMVC.Controllers
{
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
            if(_context.Product == null || _context.Category == null)
            {
                return NotFound();
            }
            
            var products = from p in _context.Product.Include(p => p.Categorie) select p;
            
            if(!string.IsNullOrEmpty(SearchTerm))
            {
                products = products.Where(p => p.Name.Contains(SearchTerm));
            }

            if(CategoryId != null)
            {
                products = products.Where(p => p.CategoryId == CategoryId);
            }
            
            
            var categories = await _context.Category.ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
            return View(await products.ToListAsync());
        }

    
    }
}