using BirthdayManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BirthdayManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly anniv_dbEntities db = new anniv_dbEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }



        public ActionResult Loginp(string login, string password)
        {
            ICollection<User> accounts = db.Users.ToList();

            string pass = CreateMD5(password);
            User a = db.Users.Where(x => x.Login == login && x.MotDePasse == pass).FirstOrDefault();

            bool IsExist = db.Users.Any(x => x.Login == login && x.MotDePasse == pass);

            if (a != null && IsExist)
            {
                /*if (a.MotDePasse == CreateMD5(a.Nom))
                    RedirectToAction("SetPassword");*/
                //Account a = db.Accounts.Where(x=>x.Login==login).FirstOrDefault();

                if (a.reserveUser == "1")
                {

                  //  User compteWiewModel = new CompteWiewModel(a.CodePersonnel, a.Personnel.Nom,
                   //     a.Personnel.Prenom, a.Personnel.NomComplet, a.Personnel.Configuration2.Libelle, a.Personnel.Email, a.Configuration.Libelle, a.Statut, "");
                    //Specialité==configuration
                    // string config = a.Personnel.Configuration.Libelle;
                    //Service==configuration1
                    // string config1 =a.Personnel.Configuration1.Libelle;
                    //TypePersonnel==configuration2
                    // string config2 =a.Personnel.Configuration2.Libelle;


                    Session["NomCompte"] = a.Nom +" "+a.Prenom;
                    Session["Prenom"] = a.Prenom;
                    Session["Email"] = a.Email;
                    Session["Role"] = a.TypeUser.Label;
                  //  Session["Photo"] = compteWiewModel.Photo;
                  //  Session["TypePersonnel"] = a.;
                   // Session["Code"] = compteWiewModel.Matricule;


                    if (a.TypeUser.Label == "Admin")
                    {

                        FormsAuthentication.SetAuthCookie(login, false);
                        return RedirectToAction("Index", "Home");
                    }
                    if (a.TypeUser.Label == "Secrétaire")
                    {

                        FormsAuthentication.SetAuthCookie(login, false);
                        return RedirectToAction("Index", "Home_sec");
                    }
                   

                }
                else
                {
                    ViewBag.State = "account";
                    return View("Login");
                }

            }
            ViewBag.State = "wrong";
            return View("Login");

        }


        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
       


        public ActionResult Logout()
        {
           // Session["nomSession"] = null;
           // StaticClass.StaticTools.status(false);
            if (Request.IsAuthenticated)
                FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }


        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}