using Maistanesys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Maistanesys.Repos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Maistanesys.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
         UserRepository userRepo = new UserRepository();
        private const string _connStrin = "Server=localhost;Database=MaistanesysDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
            return View();
        }

        [HttpPost]
        [ActionName("RegisterUser")]
        public ActionResult RegisterUser(string name, string password, int phone, string email)
        {
        //    try
          //  {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();

                if (ModelState.IsValid)
                {
                    userRepo.RegisterUser(name, password, phone, email);
                }

                return RedirectToAction("Index");
        //    }
         //   catch
         //   {
           //     return ;
          //  }
        }
    }
}