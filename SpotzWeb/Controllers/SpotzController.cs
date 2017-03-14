using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpotzWeb.Models;

namespace SpotzWeb.Controllers
{
    public class SpotzController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Spotz
        public ActionResult Index()
        {
            var vm = new List<SpotzDetailViewModel>();
            foreach (var spotz in _db.Spotzes.ToList())
            {
                vm.Add(new SpotzDetailViewModel
                    (
                    spotz
                    ));
            }
            //var allSpotz = _db.Spotzes.ToList();
            return View(vm);
        }

        // GET: Spotz/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spotz spotz = await _db.Spotzes.FindAsync(id);
            if (spotz == null)
            {
                return HttpNotFound();
            }
            return View(spotz);
        }

        // GET: Spotz/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Spotz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SpotzId,Title,Description,Longitude,Latitude,Timestamp")] Spotz spotz)
        {
            if (ModelState.IsValid)
            {
                spotz.SpotzId = Guid.NewGuid();
                _db.Spotzes.Add(spotz);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(spotz);
        }

        // GET: Spotz/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spotz spotz = await _db.Spotzes.FindAsync(id);
            if (spotz == null)
            {
                return HttpNotFound();
            }
            return View(spotz);
        }

        // POST: Spotz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SpotzId,Title,Description,Longitude,Latitude,Timestamp")] Spotz spotz)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(spotz).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(spotz);
        }

        // GET: Spotz/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spotz spotz = await _db.Spotzes.FindAsync(id);
            if (spotz == null)
            {
                return HttpNotFound();
            }
            return View(spotz);
        }

        // POST: Spotz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Spotz spotz = await _db.Spotzes.FindAsync(id);
            _db.Spotzes.Remove(spotz);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
