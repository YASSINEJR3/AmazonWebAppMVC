using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazonWebAppMVC.Controllers
{
    public class AuthController : Controller
    {

        private readonly ILogger _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            _logger.LogInformation("AuthController::Login() called");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            _logger.LogInformation(string.Format("AuthController::Login() called with username: {0} and password: {1}", username, password));
            if (username == "admin@gmail.com" && password == "admin")
            {
                _logger.LogInformation("AuthController::Login() is admin");
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Role, "Admin"),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                _logger.LogInformation("AuthController::Login() admin logged in");
                return RedirectToAction("Index", "Home");
            }
            else if (username == "client@gmail.com" && password == "client")
            {
                _logger.LogInformation("AuthController::Login() is client");
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, "Client"),
                new Claim(ClaimTypes.Role, "Client"),
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                _logger.LogInformation("AuthController::Login() client logged in");
                return RedirectToAction("Index", "Home");
            }

            // Invalid login
            return View();
        }

        //access denied
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            _logger.LogInformation("AuthController::AccessDenied() called");
            return View();
        }


        //logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("AuthController::Logout() called");
            return RedirectToAction("Login");
        }
    }
}
