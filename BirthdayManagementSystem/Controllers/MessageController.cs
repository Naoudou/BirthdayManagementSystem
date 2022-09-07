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
    public class MessageController : Controller
    {
        private anniv_dbEntities db = new anniv_dbEntities();

        // GET: Message
        public async Task<ActionResult> Index()
        {
            var messages = db.Messages.Include(m => m.NombrePersonne).Include(m => m.TypeMessage);
            return View(await messages.ToListAsync());
        }

        // GET: Message/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            ViewBag.IdNombrePersonne = new SelectList(db.NombrePersonnes, "IdNombrePersonne", "IdNombrePersonne");
            ViewBag.IdTypeMessage = new SelectList(db.TypeMessages, "IdTypeMessage", "Label");
            return View();
        }

        // POST: Message/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdMessage,Titre,CorpsMessage,CorpsMessage2,IdTypeMessage,IdNombrePersonne")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdNombrePersonne = new SelectList(db.NombrePersonnes, "IdNombrePersonne", "IdNombrePersonne", message.IdNombrePersonne);
            ViewBag.IdTypeMessage = new SelectList(db.TypeMessages, "IdTypeMessage", "Label", message.IdTypeMessage);
            return View(message);
        }

        // GET: Message/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdNombrePersonne = new SelectList(db.NombrePersonnes, "IdNombrePersonne", "IdNombrePersonne", message.IdNombrePersonne);
            ViewBag.IdTypeMessage = new SelectList(db.TypeMessages, "IdTypeMessage", "Label", message.IdTypeMessage);
            return View(message);
        }

        // POST: Message/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdMessage,Titre,CorpsMessage,CorpsMessage2,IdTypeMessage,IdNombrePersonne")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdNombrePersonne = new SelectList(db.NombrePersonnes, "IdNombrePersonne", "IdNombrePersonne", message.IdNombrePersonne);
            ViewBag.IdTypeMessage = new SelectList(db.TypeMessages, "IdTypeMessage", "Label", message.IdTypeMessage);
            return View(message);
        }

        // GET: Message/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Message message = await db.Messages.FindAsync(id);
            db.Messages.Remove(message);
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
