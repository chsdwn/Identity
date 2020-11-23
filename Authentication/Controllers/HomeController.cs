using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Ali"),
                new Claim(ClaimTypes.Email, "Ali@mail.com"),
                new Claim("Grandma.Says", "Very nice boi.")
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Ali Veli"),
                new Claim("DrivingLicense", "B")
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}