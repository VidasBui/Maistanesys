using Maistanesys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Maistanesys.Repos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;

namespace Maistanesys.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly OrdersContext _context;
        ItemRepository itemRepo = new ItemRepository();

        UserRepository userRepo=new UserRepository();

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;

            var contextOptions = new DbContextOptionsBuilder<OrdersContext>()
                .UseSqlServer(@"Server=localhost;Database=MaistanesysDB;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            _context = new OrdersContext(contextOptions);
        }

        // GET: OrderController
        public ActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            int id = userRepo.GetUserId(username);
            int orderId = userRepo.GetUserOrder(id);
            return View(itemRepo.getOrderItems(orderId));
        }

        public ActionResult OrderView(int orderId)
        {
            return View(itemRepo.getOrderItems(orderId));
        }
        public ActionResult History()
        {
            ModelState.Clear();

            return View(itemRepo.getOrders());
        }

        public ActionResult UserHistory()
        {
            ModelState.Clear();
            string username = HttpContext.Session.GetString("username");
            int id = userRepo.GetUserId(username);
            return View(itemRepo.getUserOrders(id));
        }

        public ActionResult ViewOrders()
        {
            ModelState.Clear();
            return View(itemRepo.getAllOrders());
        }

        public ActionResult ChangeState(int orderId,int state)
        {
            itemRepo.ChangeState(orderId,state);
            return RedirectToAction("ViewOrders", "Order");
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult Edit(int id)
        {
            Item item = itemRepo.getItem(id);
            return View(item);
        }

    

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id,int flag)
        {
            Item item = itemRepo.getItem(id);
            return View(item);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,int flag, IFormCollection collection)
        {
            try
            {
                string username = HttpContext.Session.GetString("username");
                int idusr = userRepo.GetUserId(username);
                int orderId = userRepo.GetUserOrder(idusr);
                itemRepo.DeleteItem(id,flag,orderId);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //state=0 neapmoketas
        //state=1 apmoketas
        //state=2 atsauktas
        //state=3 gaminama
        //state=4 pagaminta
        //state=5 pristatoma
        //state=6 pristatyta
        [HttpPost]
        public ActionResult AddToOrder(string address)
        {
            string username = HttpContext.Session.GetString("username");
            int id = userRepo.GetUserId(username);
            int orderId = userRepo.GetUserOrder(id);
            itemRepo.ChangeStateAddress(orderId, 1, address);
            itemRepo.addOrderIfNone(id);

            return RedirectToAction("Index", "Home");
        }
    }
}
