using BirthdayManagementSystem.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace BirthdayManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private anniv_dbEntities db = new anniv_dbEntities();
       
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // JobScheduler js = new JobScheduler();

            // Main();


            JobScheduler.StartAsync();

          /*  System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = (40000);
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimerElapsed);
            timer.Enabled = true;*/
        }






        int testNaiss = 1;
        int testVieReligieuse = 1;
        int testSacerdoce = 1;
        int testDernirVeux = 1;
     

        public void TimerElapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            anniv_dbEntities db = new anniv_dbEntities();
            

            List<Jesuite> jesuites = new List<Jesuite>();
            List<Jesuite> anniversereux = new List<Jesuite>();
            List<Jesuite> list_des_jubileviereligeuse = new List<Jesuite>();
            List<Jesuite> list_des_jubilesacerdoce = new List<Jesuite>();
            List<Jesuite> list_des_jubilederniervoeux = new List<Jesuite>();

            List<messageModelView> messageAnniv_singu = new List<messageModelView>();
            List<messageModelView> messageJubilesacerdoce_singu = new List<messageModelView>();
            List<messageModelView> messageJubileVieReligieuse_singu = new List<messageModelView>();
            List<messageModelView> messageRappelDernierVoeux_singu = new List<messageModelView>();

            List<messageModelView> messageAnniv_pluri = new List<messageModelView>();
            List<messageModelView> messageJubilesacerdoce_pluri = new List<messageModelView>();
            List<messageModelView> messageJubileVieReligieuse_pluri = new List<messageModelView>();
            List<messageModelView> messageRappelDernierVoeux_pluri = new List<messageModelView>();


           // List<string> gobal_list_email = new List<string>();
           // gobal_list_email.Add("anniversairepao@gmail.com, fulthonn@gmail.com, naoufulyahoo.fr");

            // string maillist = "provincialpao@gmail.com,sociuspao@yahoo.fr,communicationprovincepao@gmail.com,anniversairepao@gmail.com";
           // string maillist = "anniversairepao@gmail.com, fulthonn@gmail.com, naoufulyahoo.fr";
            string maillist_dernier_voeux = "";
            //chargement de la liste des contacts qui vont recevoir les email pour les anniversaire sauf pour les derniers voeux
            foreach (Jesuite j in db.Jesuites)
            {
                jesuites.Add(j);
                //maillist = maillist + "," + j.Email;
               
            }

            //chargement de la liste de ceux qui vont recevoir le message de rappel du jour de leur dernier voeux
            foreach (Jesuite jdv in list_des_jubilederniervoeux)
            {

                maillist_dernier_voeux = maillist_dernier_voeux + "," + jdv.Email;
            }


            //chargement des message singulier 
            foreach (Message me_an in db.Messages)
            {
                //message anniv
                if (me_an.TypeMessage.Label.Equals("Anniverssaire de naissance") && me_an.NombrePersonne.NombreAnniversereux.Equals(1))
                {
                    
                    messageAnniv_singu.Add(new messageModelView(me_an.Titre,me_an.CorpsMessage,me_an.CorpsMessage2,me_an.NombrePersonne.NombreAnniversereux,me_an.TypeMessage.Label));
                }
                //message vie religieuse
                if (me_an.TypeMessage.Label.Equals("Religieuse") && me_an.NombrePersonne.NombreAnniversereux.Equals(1))
                {
                    messageJubileVieReligieuse_singu.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));

                }
                //message vie sacerdotale
                if (me_an.TypeMessage.Label.Equals("Sacerdoce") && me_an.NombrePersonne.NombreAnniversereux.Equals(1))
                {
                    messageJubilesacerdoce_singu.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));

                }
                //message rappel dernier voeux
                if (me_an.TypeMessage.Label.Equals("Derniers voeux") && me_an.NombrePersonne.NombreAnniversereux.Equals(1))
                {
                    messageRappelDernierVoeux_singu.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));

                }

            }

           

            //chargement des message pluriel
            foreach (Message me_an in db.Messages)
            {
                //message anniv
                if (me_an.TypeMessage.Label.Equals("Anniverssaire de naissance") && me_an.NombrePersonne.NombreAnniversereux.Equals(2))
                {
                    messageAnniv_pluri.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));

                }
                //message vie religieuse
                if (me_an.TypeMessage.Label.Equals("Religieuse") && me_an.NombrePersonne.NombreAnniversereux.Equals(2))
                {
                    messageJubileVieReligieuse_pluri.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));

                }
                //message vie sacerdotale
                if (me_an.TypeMessage.Label.Equals("Sacerdoce") && me_an.NombrePersonne.NombreAnniversereux.Equals(2))
                {
                    messageJubilesacerdoce_pluri.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));

                }
                //message rappel dernier voeux
                if (me_an.TypeMessage.Label.Equals("Derniers voeux") && me_an.NombrePersonne.NombreAnniversereux.Equals(2))
                {
                    messageRappelDernierVoeux_pluri.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));

                }

            }


            //recherche des personnes qui fête leur anniversaire
            foreach (Jesuite je in jesuites)
            {
                if (je.DateNassance != null)
                {
                    if (je.DateNassance.Month.Equals(DateTime.Now.Month) && je.DateNassance.Day.Equals(DateTime.Now.Day))
                    {
                        anniversereux.Add(je);
                    }
                }
                
                if (je.EntreeNoviciat != null)
                {
                    if ( je.EntreeNoviciat.Value.Month.Equals(DateTime.Now.Month) && je.EntreeNoviciat.Value.Day.Equals(DateTime.Now.Day))
                    {
                        list_des_jubileviereligeuse.Add(je);
                    }
                }

                if (je.Ordination != null)
                {
                    if ( je.Ordination.Value.Month.Equals(DateTime.Now.Month) && je.Ordination.Value.Day.Equals(DateTime.Now.Day))
                    {
                        list_des_jubilesacerdoce.Add(je);
                    }
                }

                if (je.DernierVoeux != null)
                {
                    if ( je.DernierVoeux.Value.Month.Equals(DateTime.Now.Month) && je.DernierVoeux.Value.Day.Equals(DateTime.Now.Day))
                    {
                        list_des_jubilederniervoeux.Add(je);
                    }
                }


            }


            //Envoie des emails pour les anniversaires
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().HeureDenvoie != null && db.Settings.FirstOrDefault().HeureDeReset != null && db.Settings.FirstOrDefault().StopAll==0)
            {

                //fixation de l'heure du début de l'envoie de email
                if ((DateTime.Now.Hour.ToString().Equals(db.Settings.First().HeureDenvoie.Value.Hour.ToString()) && DateTime.Now.Minute.ToString().Equals(db.Settings.First().HeureDenvoie.Value.Minute.ToString())) && db.Settings.FirstOrDefault().AnnivNaissance == 1)
                {
                    //vérification de l'activation de l'envoie de message d'anniv de naissance
                    if (anniversereux.Count() == 1 && testNaiss == 1)
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("anniversairepao@gmail.com");
                           
                            foreach(Jesuite j  in jesuites)
                            {
                                mail.To.Add(j.Email);
                            }
                            


                            mail.Subject = "Anniversaire du compagnon " + anniversereux.FirstOrDefault().NomComplete;
                            mail.Body = messageAnniv_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                              + anniversereux.FirstOrDefault().NomComplete.ToUpper() + "\r\n " + anniversereux.FirstOrDefault().Email+" "
                                                                    + messageAnniv_singu.FirstOrDefault().CorpsMessage;
                            
                            mail.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");

                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                                testNaiss = 0;
                            }

                        }
                    }

                    //envoie de mail anniv pluriel
                    if (anniversereux.Count() > 1 && testNaiss == 1)
                    {


                        using (MailMessage mail_p = new MailMessage())
                        {
                            mail_p.From = new MailAddress("anniversairepao@gmail.com");

                            foreach (Jesuite j in jesuites)
                            {
                                mail_p.To.Add(j.Email);
                            }


                          


                            string nomDesAnniversereux = "";
                            string nomDesAnniversereuxAvecEmail = "";

                            foreach (Jesuite j in anniversereux)
                            {

                                nomDesAnniversereux = nomDesAnniversereux + "" + ", " + j.NomComplete.ToUpper();
                                nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                            }
                            mail_p.Subject = "Anniversaire des compagnons " + nomDesAnniversereux;
                            mail_p.Body = messageAnniv_pluri.FirstOrDefault().
                               Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                               + nomDesAnniversereuxAvecEmail + "\r\n"
                                                                     + messageAnniv_pluri.FirstOrDefault().CorpsMessage;

                            mail_p.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");

                                smtp.EnableSsl = true;
                                
                                smtp.Send(mail_p);
                                testNaiss = 0;
                            }

                        }
                    }







                }


            }


            //Envoie des emails pour jubilé vie religieuse
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().HeureDenvoie != null && db.Settings.FirstOrDefault().HeureDeReset != null && db.Settings.FirstOrDefault().StopAll == 0)
            {

                //fixation de l'heure du début de l'envoie de email
                if ((DateTime.Now.Hour.ToString().Equals(db.Settings.First().HeureDenvoie.Value.Hour.ToString()) && DateTime.Now.Minute.ToString().Equals(db.Settings.First().HeureDenvoie.Value.Minute.ToString())) && db.Settings.FirstOrDefault().AnnivVieReligieuse == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubileviereligeuse.Count() == 1 && testVieReligieuse == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubileviereligeuse.FirstOrDefault().EntreeNoviciat.Value.Year;

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("anniversairepao@gmail.com");

                            foreach (Jesuite j in jesuites)
                            {
                                mail.To.Add(j.Email);
                            }


                            mail.Subject = "Jubilé de la vie religieuse du compagnon " + list_des_jubileviereligeuse.FirstOrDefault().NomComplete;
                            mail.Body = messageJubileVieReligieuse_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les "+annee+" ans de vie réligieuse de "+
                              list_des_jubileviereligeuse.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubileviereligeuse.FirstOrDefault().Email + ") "
                                                                    + messageJubileVieReligieuse_singu.FirstOrDefault().CorpsMessage;

                            mail.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");

                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                                testVieReligieuse = 0;
                            }

                        }
                    }

                    //envoie de mail jubilé pluriel
                    if (list_des_jubileviereligeuse.Count() > 1 && testVieReligieuse == 1)
                    {

                        int annee = DateTime.Now.Year - list_des_jubileviereligeuse.FirstOrDefault().EntreeNoviciat.Value.Year;

                        using (MailMessage mail_p = new MailMessage())
                        {
                            mail_p.From = new MailAddress("anniversairepao@gmail.com");

                            foreach (Jesuite j in jesuites)
                            {
                                mail_p.To.Add(j.Email);
                            }

                            string nomDesjubileVieReligieuse = "";
                            string nomSansEmailDesjubileVieReligieuse = "";

                            foreach (Jesuite j in list_des_jubileviereligeuse)
                            {

                                nomDesjubileVieReligieuse = nomDesjubileVieReligieuse + "" + ", " + j.NomComplete.ToUpper() + " ( "+ j.Email +" )";
                                nomSansEmailDesjubileVieReligieuse = nomSansEmailDesjubileVieReligieuse + "" + ", " + j.NomComplete;
                               
                            }

                            mail_p.Subject = "Jubilé de la vie religieuse des compagnons " + nomSansEmailDesjubileVieReligieuse;
                            mail_p.Body = messageJubileVieReligieuse_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de vie réligieuse de " +
                              nomDesjubileVieReligieuse   + messageJubileVieReligieuse_pluri.FirstOrDefault().CorpsMessage; 


                            mail_p.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");

                                smtp.EnableSsl = true;

                                smtp.Send(mail_p);
                                testVieReligieuse = 0;
                            }

                        }
                    }







                }


            }



            //Envoie des emails pour jubilé vie sacerdoce
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().HeureDenvoie != null && db.Settings.FirstOrDefault().HeureDeReset != null && db.Settings.FirstOrDefault().StopAll == 0)
            {

                //fixation de l'heure du début de l'envoie de email
                if ((DateTime.Now.Hour.ToString().Equals(db.Settings.First().HeureDenvoie.Value.Hour.ToString()) && DateTime.Now.Minute.ToString().Equals(db.Settings.First().HeureDenvoie.Value.Minute.ToString())) && db.Settings.FirstOrDefault().AnnivSacerdoce == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilesacerdoce.Count() == 1 && testSacerdoce == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("anniversairepao@gmail.com");

                            foreach (Jesuite j in jesuites)
                            {
                                mail.To.Add(j.Email);
                            }


                            mail.Subject = "Jubilé de sacerdoce du compagnon " + list_des_jubilesacerdoce.FirstOrDefault().NomComplete;
                            mail.Body = messageJubilesacerdoce_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de sacerdoce de " +
                              list_des_jubilesacerdoce.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubilesacerdoce.FirstOrDefault().Email + ") "
                                                                    + messageJubilesacerdoce_singu.FirstOrDefault().CorpsMessage;

                            mail.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");

                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                                testSacerdoce = 0;
                            }

                        }
                    }

                    //envoie de mail jubilé pluriel
                    if (list_des_jubilesacerdoce.Count() > 1 && testSacerdoce == 1)
                    {

                        int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;

                        using (MailMessage mail_p = new MailMessage())
                        {
                            mail_p.From = new MailAddress("anniversairepao@gmail.com");

                            foreach (Jesuite j in jesuites)
                            {
                                mail_p.To.Add(j.Email);
                            }

                            string nomDesjubilesacerdoce = "";
                            string nomSansEmailDesjubileSacerdoce = "";

                            foreach (Jesuite j in list_des_jubilesacerdoce)
                            {

                                nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                                nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + j.NomComplete;

                            }

                            mail_p.Subject = "Jubilé de sacerdoce des compagnons " + nomSansEmailDesjubileSacerdoce;
                            mail_p.Body = messageJubilesacerdoce_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de sacerdoce des compagnons " +
                              nomDesjubilesacerdoce +". "+ messageJubilesacerdoce_pluri.FirstOrDefault().CorpsMessage;


                            mail_p.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");

                                smtp.EnableSsl = true;

                                smtp.Send(mail_p);
                                testSacerdoce = 0;
                            }

                        }
                    }







                }


            }

            //Envoie des emails pour jubilé derniers voeux
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().HeureDenvoie != null && db.Settings.FirstOrDefault().HeureDeReset != null && db.Settings.FirstOrDefault().StopAll == 0)
            {

                //fixation de l'heure du début de l'envoie de email
                if ((DateTime.Now.Hour.ToString().Equals(db.Settings.First().HeureDenvoie.Value.Hour.ToString()) && DateTime.Now.Minute.ToString().Equals(db.Settings.First().HeureDenvoie.Value.Minute.ToString())) && db.Settings.FirstOrDefault().AnnivDernierVoeux == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilederniervoeux.Count() > 0 && testDernirVeux == 1)
                    {

                        foreach(Jesuite j in list_des_jubilederniervoeux)
                        {
                            //recupération du nombre d'année dernier voeux
                            int annee = DateTime.Now.Year - j.DernierVoeux.Value.Year;
                            using (MailMessage mail = new MailMessage())
                            {
                                mail.From = new MailAddress("anniversairepao@gmail.com");

                                
                                    mail.To.Add(j.Email);


                                mail.Subject = "Rappel de tes derniers voeux";
                                mail.Body = messageRappelDernierVoeux_singu.FirstOrDefault().
                                  Titre + " \r\n " + ". Aujourd'hui " + " " + DateTime.Now.ToLongDateString().ToString() + " marque l'anniversaire de tes derniers voeux. Cela fait déjà " + annee + " année depuis votre incorporation définitive dans la Compagnie de Jésus "+
                                   "\r\n" + messageRappelDernierVoeux_singu.FirstOrDefault().CorpsMessage;

                                mail.IsBodyHtml = true;

                                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                {
                                    smtp.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");

                                    smtp.EnableSsl = true;
                                    smtp.Send(mail);
                                    testDernirVeux = 0;
                                }

                            }
                        }
                                                                      
                    }
                                      

                }


            }

        }
    }
}
