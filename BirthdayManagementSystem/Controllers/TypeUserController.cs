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
    public class TypeUserController : Controller
    {
        private anniv_dbEntities db = new anniv_dbEntities();

        // GET: TypeUser
        public async Task<ActionResult> Index()
        {
            return View(await db.TypeUsers.ToListAsync());
        }

        // GET: TypeUser/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeUser typeUser = await db.TypeUsers.FindAsync(id);
            if (typeUser == null)
            {
                return HttpNotFound();
            }
            return View(typeUser);
        }

        // GET: TypeUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeUser/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdTypeUser,Label")] TypeUser typeUser)
        {
            if (ModelState.IsValid)
            {
                db.TypeUsers.Add(typeUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(typeUser);
        }

        // GET: TypeUser/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeUser typeUser = await db.TypeUsers.FindAsync(id);
            if (typeUser == null)
            {
                return HttpNotFound();
            }
            return View(typeUser);
        }

        // POST: TypeUser/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdTypeUser,Label")] TypeUser typeUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(typeUser);
        }

        // GET: TypeUser/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeUser typeUser = await db.TypeUsers.FindAsync(id);
            if (typeUser == null)
            {
                return HttpNotFound();
            }
            return View(typeUser);
        }

        // POST: TypeUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TypeUser typeUser = await db.TypeUsers.FindAsync(id);
            db.TypeUsers.Remove(typeUser);
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
