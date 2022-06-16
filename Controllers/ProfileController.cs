using Maistanesys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Maistanesys.Repos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Maistanesys.Controllers
{
    public class ProfileController : Controller
    {

        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
