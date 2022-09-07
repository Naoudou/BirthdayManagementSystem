using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BirthdayManagementSystem.Models;

namespace BirthdayManagementSystem.Controllers
{
    [Authorize]
    public class Jesuite_secController : Controller
    {
        private anniv_dbEntities db = new anniv_dbEntities();

        // GET: Jesuite
        public async Task<ActionResult> Index()
        {
            return View(await db.Jesuites.ToListAsync());
        }

        // GET: Jesuite/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jesuite jesuite = await db.Jesuites.FindAsync(id);
            if (jesuite == null)
            {
                return HttpNotFound();
            }
            return View(jesuite);
        }

        // GET: Jesuite/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jesuite/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdJesuite,Nom,Prenom,NomComplete,Email,Telephone,DateNassance,EntreeNoviciat,PremierVoeux,Ordination,DernierVoeux")] Jesuite jesuite)
        {

            jesuite.NomComplete = jesuite.Nom + " " + jesuite.Prenom;
            jesuite.Telephone = "7777777";
            if (ModelState.IsValid)
            {
                db.Jesuites.Add(jesuite);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(jesuite);
        }

        // GET: Jesuite/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jesuite jesuite = await db.Jesuites.FindAsync(id);
            if (jesuite == null)
            {
                return HttpNotFound();
            }
            return View(jesuite);
        }

        // POST: Jesuite/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdJesuite,Nom,Prenom,NomComplete,Email,Telephone,DateNassance,EntreeNoviciat,PremierVoeux,Ordination,DernierVoeux")] Jesuite jesuite)
        {
            jesuite.NomComplete = jesuite.Nom + " " + jesuite.Prenom;
            jesuite.Telephone = "7777777";
            if (ModelState.IsValid)
            {
                db.Entry(jesuite).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(jesuite);
        }

        // GET: Jesuite/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jesuite jesuite = await db.Jesuites.FindAsync(id);
            if (jesuite == null)
            {
                return HttpNotFound();
            }
            return View(jesuite);
        }

        // POST: Jesuite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Jesuite jesuite = await db.Jesuites.FindAsync(id);
            db.Jesuites.Remove(jesuite);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
