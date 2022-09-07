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
    public class NombrePersonneController : Controller
    {
        private anniv_dbEntities db = new anniv_dbEntities();

        // GET: NombrePersonne
        public async Task<ActionResult> Index()
        {
            return View(await db.NombrePersonnes.ToListAsync());
        }

        // GET: NombrePersonne/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NombrePersonne nombrePersonne = await db.NombrePersonnes.FindAsync(id);
            if (nombrePersonne == null)
            {
                return HttpNotFound();
            }
            return View(nombrePersonne);
        }

        // GET: NombrePersonne/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NombrePersonne/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdNombrePersonne,NombreAnniversereux")] NombrePersonne nombrePersonne)
        {
            if (ModelState.IsValid)
            {
                db.NombrePersonnes.Add(nombrePersonne);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(nombrePersonne);
        }

        // GET: NombrePersonne/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NombrePersonne nombrePersonne = await db.NombrePersonnes.FindAsync(id);
            if (nombrePersonne == null)
            {
                return HttpNotFound();
            }
            return View(nombrePersonne);
        }

        // POST: NombrePersonne/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdNombrePersonne,NombreAnniversereux")] NombrePersonne nombrePersonne)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nombrePersonne).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nombrePersonne);
        }

        // GET: NombrePersonne/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NombrePersonne nombrePersonne = await db.NombrePersonnes.FindAsync(id);
            if (nombrePersonne == null)
            {
                return HttpNotFound();
            }
            return View(nombrePersonne);
        }

        // POST: NombrePersonne/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NombrePersonne nombrePersonne = await db.NombrePersonnes.FindAsync(id);
            db.NombrePersonnes.Remove(nombrePersonne);
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
