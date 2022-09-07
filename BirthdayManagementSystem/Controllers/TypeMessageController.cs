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
    public class TypeMessageController : Controller
    {
        private anniv_dbEntities db = new anniv_dbEntities();

        // GET: TypeMessage
        public async Task<ActionResult> Index()
        {
            return View(await db.TypeMessages.ToListAsync());
        }

        // GET: TypeMessage/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeMessage typeMessage = await db.TypeMessages.FindAsync(id);
            if (typeMessage == null)
            {
                return HttpNotFound();
            }
            return View(typeMessage);
        }

        // GET: TypeMessage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeMessage/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdTypeMessage,Label")] TypeMessage typeMessage)
        {
            if (ModelState.IsValid)
            {
                db.TypeMessages.Add(typeMessage);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(typeMessage);
        }

        // GET: TypeMessage/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeMessage typeMessage = await db.TypeMessages.FindAsync(id);
            if (typeMessage == null)
            {
                return HttpNotFound();
            }
            return View(typeMessage);
        }

        // POST: TypeMessage/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdTypeMessage,Label")] TypeMessage typeMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeMessage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(typeMessage);
        }

        // GET: TypeMessage/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeMessage typeMessage = await db.TypeMessages.FindAsync(id);
            if (typeMessage == null)
            {
                return HttpNotFound();
            }
            return View(typeMessage);
        }

        // POST: TypeMessage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TypeMessage typeMessage = await db.TypeMessages.FindAsync(id);
            db.TypeMessages.Remove(typeMessage);
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
