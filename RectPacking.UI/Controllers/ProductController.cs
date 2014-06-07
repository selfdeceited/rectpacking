using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RectPacking.UI.Models;

namespace RectPacking.UI.Controllers
{
    public class ProductController : Controller
    {
        private PackingContext db = new PackingContext();

        //
        // GET: /Product/

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Image).Include(p => p.ConcreteType);
            return View(products.ToList());
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(long id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            ViewBag.ImageId = new SelectList(db.Images, "Id", "ImageUrl");
            ViewBag.ConcreteTypeId = new SelectList(db.ConcreteTypes, "Id", "Name");
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ImageId = new SelectList(db.Images, "Id", "ImageUrl", product.ImageId);
            ViewBag.ConcreteTypeId = new SelectList(db.ConcreteTypes, "Id", "Name", product.ConcreteTypeId);
            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageId = new SelectList(db.Images, "Id", "ImageUrl", product.ImageId);
            ViewBag.ConcreteTypeId = new SelectList(db.ConcreteTypes, "Id", "Name", product.ConcreteTypeId);
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImageId = new SelectList(db.Images, "Id", "ImageUrl", product.ImageId);
            ViewBag.ConcreteTypeId = new SelectList(db.ConcreteTypes, "Id", "Name", product.ConcreteTypeId);
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}