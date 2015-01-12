using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BillBoard.Models;
using System.Web.Security;

namespace BillBoard.Controllers
{
    public class AdvertController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public int pageSize = 4;

        //
        // GET: /Advert/
        public ActionResult List(string category, string find, int page = 1)
        {
            TempData["search"] = find;
            var adverts = db.Adverts.Include(a => a.Category).Include(a => a.Type);
            int TotalItem;

            if (!String.IsNullOrEmpty(find))
            {
                adverts = adverts.Where(a => a.Title.Contains(find));
                TotalItem = adverts.OrderByDescending(a => a.AdvertID).Skip((page - 1) * pageSize).Take(pageSize).ToList().Count();
            }
            else
            {
                TotalItem = category == null ? db.Adverts.Count() : db.Adverts.Where(e => e.Category.Name == category).Count();
            }

            AdvertsListViewModel model = new AdvertsListViewModel
            {
                Adverts = adverts
                .Where(a => category == null || a.Category.Name == category)
                .OrderByDescending(a => a.AdvertID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = TotalItem
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public PartialViewResult Navigation(string category = null)
        {
            ViewBag.SelectedCategory = category;
            return PartialView(db.Categories.ToList());
        }

        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.Adverts = "Все объявления";
                return View(db.Adverts.Include(a => a.Category).Include(a => a.Type).OrderByDescending(a => a.AdvertID).ToList());
            }
            else
            {
                ViewBag.Adverts = "Мои объявления";
                MembershipUser mu = Membership.GetUser(User.Identity.Name);
                return View(db.Adverts.Include(a => a.Category).Include(a => a.Type).
                    Where(a => a.UserId == (int)mu.ProviderUserKey).ToList());
            }
        }

        //
        // GET: /Advert/Details/5

        public ActionResult Details(int id = 0)
        {
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        //
        // GET: /Advert/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Name");
            return View();
        }

        //
        // POST: /Advert/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Advert advert, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    advert.ImageMimeType = image.ContentType;
                    advert.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(advert.ImageData, 0, image.ContentLength);
                }
                MembershipUser mu = Membership.GetUser(User.Identity.Name);
                advert.UserId = Convert.ToInt32(mu.ProviderUserKey);
                db.Adverts.Add(advert);
                db.SaveChanges();
                TempData["message"] = string.Format("{0} был сохранен", advert.Title);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", advert.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Name", advert.TypeID);
            return View(advert);
        }

        //
        // GET: /Advert/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", advert.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Name", advert.TypeID);
            return View(advert);
        }

        //
        // POST: /Advert/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Advert advert, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    advert.ImageMimeType = image.ContentType;
                    advert.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(advert.ImageData, 0, image.ContentLength);
                }

                MembershipUser mu = Membership.GetUser(User.Identity.Name);
                advert.UserId = Convert.ToInt32(mu.ProviderUserKey);

                db.Entry(advert).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = string.Format("{0} был сохранен", advert.Title);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", advert.CategoryID);
            ViewBag.TypeID = new SelectList(db.Types, "TypeID", "Name", advert.TypeID);
            return View(advert);
        }

        //
        // GET: /Advert/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        //
        // POST: /Advert/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advert advert = db.Adverts.Find(id);
            db.Adverts.Remove(advert);
            db.SaveChanges();
            TempData["message"] = string.Format("{0} был удален", advert.Title);
            return RedirectToAction("Index");
        }

        public FileContentResult GetImage(int AdvertID)
        {
            Advert advert = db.Adverts.FirstOrDefault(a => a.AdvertID == AdvertID);
            if (advert != null)
            {
                return File(advert.ImageData, advert.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}