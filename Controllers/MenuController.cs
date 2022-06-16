using Maistanesys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Maistanesys.Repos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Maistanesys.Controllers
{
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private readonly OrdersContext _context;
        private const string _connStrin = "Server=localhost;Database=MaistanesysDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        ItemRepository itemRepo = new ItemRepository();
        UserRepository userRepo=new UserRepository();

        public MenuController(ILogger<MenuController> logger)
        {
            _logger = logger;

            var contextOptions = new DbContextOptionsBuilder<OrdersContext>()
                .UseSqlServer(_connStrin)
                .Options;

            _context = new OrdersContext(contextOptions);
        }
        // GET: OrderController/Edit/5
        [HttpPost]
        [ActionName("EditForRestaurant")]
        public ActionResult EditForRestaurant(int id)
        {
            Item item = itemRepo.getItem(id);
            return View(item);
        }
        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRestaurant(Item collection)
        {
            string username = HttpContext.Session.GetString("username");
            int idusr = userRepo.GetUserId(username);
            
            int orderId = 0;
            orderId = userRepo.GetUserOrder(idusr);

            try
            {
                if (ModelState.IsValid)
                {
                    itemRepo.UpdateItemForRestaurant(collection);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(collection);
            }
        }
        public IActionResult Index()
        {
            ModelState.Clear();

            return View(itemRepo.getItems());
        }

        public ActionResult Create()
        {
            Item item = new Item(); 
            return View(item);
        }

        [HttpPost]
        public ActionResult Create(string name, float price, int cat)
        {
            if (ModelState.IsValid)
            {
                itemRepo.addToMenu(name, price, cat);
                return RedirectToAction("Index");
            }
            else
            {
                Item item = new Item();
                return View(item);
            }
            
        }

        [HttpPost]
        [ActionName("AddToOrder")]
        public ActionResult AddToOrder(int item, int quantity)
        {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                string username = HttpContext.Session.GetString("username");
                int id = userRepo.GetUserId(username);
                int orderId = userRepo.GetUserOrder(id);

            if (ModelState.IsValid)
                {
                    itemRepo.addMenu(item, orderId, quantity);
                }

                return RedirectToAction("Index");

        }

        public ActionResult Edit(int id)
        {
            Item item = itemRepo.getItem(id);
            return View(item);
        }

        /*[HttpPost]
        public ActionResult Edit(int id, Item collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    itemRepo.UpdateItem(collection);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(collection);
            }
        }*/

        public ActionResult Delete(int id)
        {
            Item item = itemRepo.getItem(id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(int id,int flag, IFormCollection collection)
        {
            string username = HttpContext.Session.GetString("username");
            int idusr = userRepo.GetUserId(username);
            int orderId = userRepo.GetUserOrder(idusr);
            try
            {
                itemRepo.DeleteItem(id,flag,orderId);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
