using Maistanesys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Maistanesys.Repos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace Maistanesys.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        UserRepository userRepo = new UserRepository();
        ItemRepository itemRepo = new ItemRepository();
        private const string _connStrin = "Server=localhost;Database=MaistanesysDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("LoginUser")]
        public ActionResult LoginUser(string name, string password, int phone, string email)
        {
            User logged= userRepo.GetUser(name, password);

            if (logged.Name != null)
            {
                HttpContext.Session.SetString("username", logged.Name);
                HttpContext.Session.SetString("ID", logged.Id.ToString());
                HttpContext.Session.SetString("Admin", logged.IsAdmin.ToString());
                HttpContext.Session.SetString("Email", logged.Email);
                HttpContext.Session.SetString("Phone", logged.Phone.ToString());
                itemRepo.addOrderIfNone(logged.Id);
            }

            return RedirectToAction("Index","Home");

        }

        [ActionName("LogOut")]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

    }
}