using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using RectPacking.UI.Models;

namespace RectPacking.UI.Controllers
{
    public class OrderController : Controller
    {
        private PackingContext db = new PackingContext();

        //
        // GET: /Order/

        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.User).Include(o => o.Client);
            return View(orders.ToList());
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(long id = 0)
        {
            Order order = db.Orders.Find(id);

            var pss = db.ProductSets.Where(ps => ps.OrderId == id).Select(ps => ps).ToList();
            ViewBag.tsList = pss.Select(ps => db.Products.FirstOrDefault(p => p.Id == ps.ProductId)).ToList();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order); 
        }

        //
        // GET: /Order/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "ClientName");
            return View();
        }

        //
        // POST: /Order/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {

            var productSets = Request.Form.AllKeys.Where(k => k.StartsWith("ff"));
            var inputs = Request.Form.AllKeys.Where(k => k.StartsWith("imp"));
            if (ModelState.IsValid)
            {
                //to prevent multiple enumeration
                var ps = productSets as string[] ?? productSets.ToArray();
                var imp = inputs as string[] ?? inputs.ToArray();
                var count = ps.Count();

                for (var index = 0; index < count; index++)
                {
                    var productSet = new ProductSet()
                    {
                        ProductId = Convert.ToInt64(Request.Form[ps.ElementAt(index)]),
                        OrderId = order.Id,
                        Quantity = Convert.ToInt32(Request.Form[imp.ElementAt(index)])
                    };
                    db.ProductSets.Add(productSet);
                }
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", order.UserId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "ClientName", order.ClientId);
            return View(order);
        }

        //
        // GET: /Order/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", order.UserId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "ClientName", order.ClientId);
            return View(order);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", order.UserId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "ClientName", order.ClientId);
            return View(order);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [WebMethod]
        public object GetProduct()
        {
            var simpleProductList = db.Products.Select(p => new SimpleProduct
            {
                Id = p.Id,
                Name = p.StandardName,
                Width = p.Width,
                Height = p.Height
            }).ToList();
            return Json(simpleProductList); //, JsonRequestBehavior.AllowGet);
        } 
    }

    public class SimpleProduct
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}