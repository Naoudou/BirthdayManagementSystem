using BirthdayManagementSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BirthdayManagementSystem.Controllers
{
   
    public class SenderController : Controller
    {

       
        public int FulthonnM()
        {
            SendFirst100();

            return 100;
        }


        AlternateView Mail_Body(string s)
        {
            string path = Server.MapPath(@"~\Content\assets\img\footer.png");
            string path2 = Server.MapPath(@"~\Content\assets\img\anniv_header.png");
            LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
            LinkedResource Img2 = new LinkedResource(path2, MediaTypeNames.Image.Jpeg);
            Img.ContentId = "MyImage";
            Img2.ContentId = "MyImage2";
            string str = @"  
                            <table> 

                                 <tr>  
                                    <td>  
                                      <img src=cid:MyImage2  id='img' alt='' width='100%''/>   <br/> <br/>
                                <br/>
                                    </td>  
                                </tr>
                                
                                <tr>  
                                    <td> '" + s + @"'  
                                    </td>  
                                </tr> 
                                  
                                <tr>  
                                    <td> 
                                         <br/>
                                <br/>
                                      <img src=cid:MyImage  id='img' alt='' width='100%''/>   
                                    </td>  
                                </tr></table>  
                            ";
            AlternateView AV =
            AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
            AV.LinkedResources.Add(Img);
            AV.LinkedResources.Add(Img2);
            return AV;
        }

        public int SendFirst100()
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

            //liste brute des sj
            List<Jesuite> jesuites_for_email__1 = db.Jesuites.ToList();

            //liste 100 premiers 
            List<Jesuite> jesuites_for_email = new List<Jesuite>();

            jesuites_for_email__1.OrderByDescending(x=>x.Nom);

            if (jesuites_for_email__1.Count() >= 60)
            {
                for (int i = 0; i <= 60; i++) //60
                {
                    jesuites_for_email.Add(jesuites_for_email__1.ElementAt(i));
                }
            }
            else
            {
                jesuites_for_email = jesuites_for_email__1;
            }

           

            string maillist_dernier_voeux = "";
            string maillist_global_firt_100 = "";
            //chargement de la liste des contacts qui vont recevoir les email pour les anniversaire sauf pour les derniers voeux


            foreach (Jesuite j in db.Jesuites)
            {
                jesuites.Add(j);
                //maillist = maillist + "," + j.Email;

            }


            //chargement de la liste de ceux qui vont recevoir le message anniv
            foreach (Jesuite j in jesuites_for_email)
            {

                maillist_global_firt_100 = maillist_global_firt_100 + "," + j.Email;
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

                    messageAnniv_singu.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));
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

                if (je.EntreeNoviciat.HasValue)
                {
                    if (je.EntreeNoviciat.Value.Month.Equals(DateTime.Now.Month) && je.EntreeNoviciat.Value.Day.Equals(DateTime.Now.Day) && ((DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 25) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 30) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 35) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 40) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 45) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 50) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 55) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 60) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 65) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 70) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 75) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 80) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 85) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 90)))
                    {
                        list_des_jubileviereligeuse.Add(je);
                    }
                }

                if (je.Ordination.HasValue)
                {
                    if (je.Ordination.Value.Month.Equals(DateTime.Now.Month) && je.Ordination.Value.Day.Equals(DateTime.Now.Day) && ((DateTime.Now.Year - je.Ordination.Value.Year == 25) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 30) || (DateTime.Now.Year - je.Ordination.Value.Year == 35) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 40) || (DateTime.Now.Year - je.Ordination.Value.Year == 45) || (DateTime.Now.Year - je.Ordination.Value.Year == 50) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 55) || (DateTime.Now.Year - je.Ordination.Value.Year == 60) || (DateTime.Now.Year - je.Ordination.Value.Year == 65) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 70) || (DateTime.Now.Year - je.Ordination.Value.Year == 75) || (DateTime.Now.Year - je.Ordination.Value.Year == 80) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 85) || (DateTime.Now.Year - je.Ordination.Value.Year == 90)))
                    {
                        list_des_jubilesacerdoce.Add(je);
                    }
                }


                if (je.DernierVoeux.HasValue)
                {
                    if (je.DernierVoeux.Value.Month.Equals(DateTime.Now.Month) && je.DernierVoeux.Value.Day.Equals(DateTime.Now.Day))
                    {
                        list_des_jubilederniervoeux.Add(je);
                    }
                }


            }


           
            // List<Jesuite> b = jesuites_for_email.Count();
            int a = 2;

            //Envoie des emails pour les anniversaires
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
           {

               // fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivNaissance == 1)
                {
                    //vérification de l'activation de l'envoie de message d'anniv de naissance
                    if (anniversereux.Count() == 1)
                    {

                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {
                            
                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper();
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }


                         

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            //maillist_global_firt_100
                            ms.To.Add("provincialpao@gmail.com,sociuspao@yahoo.fr");
                            ms.Bcc.Add("communicationprovincepao@gmail.com");
                            ms.Bcc.Add(maillist_global_firt_100);
                            ms.Subject = "Anniversaire du compagnon " + anniversereux.FirstOrDefault().NomComplete;
                           
                            string Body = messageAnniv_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                              + anniversereux.FirstOrDefault().NomComplete.ToUpper() + "\r\n " + anniversereux.FirstOrDefault().Email + " "
                                                                    + messageAnniv_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "xbgbclhcwnpspwto");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }

                        }



                    }



                    //    //envoie de mail anniv pluriel
                    if (anniversereux.Count() > 1)
                    {


                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper()+", ";
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }

                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                           
                            ms.To.Add("provincialpao@gmail.com,sociuspao@yahoo.fr");
                            ms.Bcc.Add("communicationprovincepao@gmail.com");
                            ms.Bcc.Add(maillist_global_firt_100);

                            ms.Subject = "Anniversaire des compagnons " + nomDesAnniversereux;
                            string Body = messageAnniv_pluri.FirstOrDefault().
                               Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                               + nomDesAnniversereuxAvecEmail + "\r\n"
                                                                     + messageAnniv_pluri.FirstOrDefault().CorpsMessage;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;

                            /* using (SmtpClient smtpClient = new SmtpClient("mail47.lwspanel.com"))
                             {
                                 smtpClient.Credentials = new NetworkCredential("anniversairepao@hebergementsjaoc.com", "Naoudou21@");
                                 smtpClient.Port = 465;
                                 smtpClient.EnableSsl = true;
                                 smtpClient.Send(ms);
                             }*/

                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "xbgbclhcwnpspwto");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }

                        }




                    }



                }


            }


           

            //Envoie des emails pour jubilé vie religieuse
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivVieReligieuse == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubileviereligeuse.Count() == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubileviereligeuse.FirstOrDefault().EntreeNoviciat.Value.Year;


                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + ""+ j.NomComplete.ToUpper();
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }
                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add("provincialpao@gmail.com,sociuspao@yahoo.fr");
                            ms.Bcc.Add("communicationprovincepao@gmail.com");
                            ms.Bcc.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de la vie religieuse du compagnon " + list_des_jubileviereligeuse.FirstOrDefault().NomComplete;
                            string Body = messageJubileVieReligieuse_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de vie réligieuse de " +
                              list_des_jubileviereligeuse.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubileviereligeuse.FirstOrDefault().Email + ") "
                                                                    + messageJubileVieReligieuse_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }
                    }


                    //envoie de mail jubilé pluriel
                    if (list_des_jubileviereligeuse.Count() > 1)
                    {

                       
                        string nomDesjubileVieReligieuse = "";
                        string nomSansEmailDesjubileVieReligieuse = "";

                        foreach (Jesuite j in list_des_jubileviereligeuse)
                        {

                            nomDesjubileVieReligieuse = nomDesjubileVieReligieuse + "" + ", " + j.NomComplete.ToUpper() + " " + (DateTime.Now.Year - j.EntreeNoviciat.Value.Year) + " " + " ans de vie réligieuse" + " " + "( " + j.Email + ")";
                            nomSansEmailDesjubileVieReligieuse = nomSansEmailDesjubileVieReligieuse + "" + ", " + j.NomComplete;

                        }
                       
                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add("provincialpao@gmail.com,sociuspao@yahoo.fr");
                            ms.Bcc.Add("communicationprovincepao@gmail.com");
                            ms.Bcc.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de la vie religieuse des compagnons " + nomSansEmailDesjubileVieReligieuse;
                            string Body = messageJubileVieReligieuse_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les jubilés de la vie réligieuse des compagnons " +
                              nomDesjubileVieReligieuse + ". " + messageJubileVieReligieuse_pluri.FirstOrDefault().CorpsMessage;


                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }

                    }



                }


            }



            //Envoie des emails pour jubilé vie sacerdoce
            //verification du statut de la plateforme: en marche ou stop
            if (db.Settings.FirstOrDefault().StopAll==1)
            {

                //vérification du statut de anniv sacerdoce
                if (db.Settings.FirstOrDefault().AnnivSacerdoce == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilesacerdoce.Count() == 1 )
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;



                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add("provincialpao@gmail.com,sociuspao@yahoo.fr");
                            ms.Bcc.Add("communicationprovincepao@gmail.com");
                            ms.Bcc.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de sacerdoce du compagnon " + list_des_jubilesacerdoce.FirstOrDefault().NomComplete;
                            string Body = messageJubilesacerdoce_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de sacerdoce de " +
                              list_des_jubilesacerdoce.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubilesacerdoce.FirstOrDefault().Email + ") "
                                                                    + messageJubilesacerdoce_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }
                           
                       


                    }

                    //envoie de mail jubilé pluriel
                    if (list_des_jubilesacerdoce.Count() > 1 )
                    {

                       // int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;

                       

                       
                            string nomDesjubilesacerdoce = "";
                            string nomSansEmailDesjubileSacerdoce = "";

                            foreach (Jesuite j in list_des_jubilesacerdoce)
                            {

                                nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + j.NomComplete.ToUpper() + " " + (DateTime.Now.Year - j.Ordination.Value.Year) + " " + " ans de sacerdoce" + " " + "( " + j.Email + ")";
                            nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + j.NomComplete;

                            }


                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add("provincialpao@gmail.com,sociuspao@yahoo.fr");
                            ms.Bcc.Add("communicationprovincepao@gmail.com");
                            ms.Bcc.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de sacerdoce des compagnons " + nomSansEmailDesjubileSacerdoce;
                            string Body = messageJubilesacerdoce_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les jubilés de sacerdoce des compagnons"+" "+
                              nomDesjubilesacerdoce + ". " + messageJubilesacerdoce_pluri.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;

                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }
                           

                    }


                }


            }

           
            //Envoie des emails pour jubilé derniers voeux
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivDernierVoeux == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilederniervoeux.Count() > 0 )
                    {

                        foreach (Jesuite j in list_des_jubilederniervoeux)
                        {
                            //recupération du nombre d'année dernier voeux
                            int annee = DateTime.Now.Year - j.DernierVoeux.Value.Year;
                            //using (MailMessage mail = new MailMessage())
                           

                           
                                string nomDesjubilesacerdoce = "";
                                string nomSansEmailDesjubileSacerdoce = "";

                                foreach (Jesuite je in list_des_jubilesacerdoce)
                                {

                                    nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + je.NomComplete.ToUpper() + " ( " + je.Email + " )";
                                    nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + je.NomComplete;

                                }

                               

                            using (MailMessage ms = new MailMessage())
                            {
                                ms.From = new MailAddress("anniversairepao@gmail.com");

                                ms.To.Add(j.Email);

                                 ms.To.Add("naouful@yahoo.fr");
                                ms.Subject = "Rappel de tes derniers voeux";
                                string Body = messageRappelDernierVoeux_singu.FirstOrDefault().
                                  Titre + " \r\n " + ". Aujourd'hui " + " " + DateTime.Now.ToLongDateString().ToString() + " marque l'anniversaire de tes derniers voeux. Cela fait déjà " + annee + " année depuis votre incorporation définitive dans la Compagnie de Jésus " +
                                   "\r\n" + messageRappelDernierVoeux_singu.FirstOrDefault().CorpsMessage;

                                ms.AlternateViews.Add(Mail_Body(Body));
                                ms.IsBodyHtml = true;

                                ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                                {
                                    smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                    //smtpClient.Port = 25;
                                    smtpClient.EnableSsl = true;
                                    smtpClient.Send(ms);
                                }
                            }
                                
                            
                        }

                    }


                }


            }
            return 100;
        }


        public int SendFirst200()
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

            //liste brute des sj
            List<Jesuite> jesuites_for_email__1 = db.Jesuites.ToList();

            //liste 100 premiers 
            List<Jesuite> jesuites_for_email = new List<Jesuite>();

            jesuites_for_email__1.OrderByDescending(x => x.Nom);

            if (jesuites_for_email__1.Count() >= 130)
            {
                for (int i = 61; i <= 130; i++)
                {
                    jesuites_for_email.Add(jesuites_for_email__1.ElementAt(i));
                }
            }

            


            string maillist_dernier_voeux = "";
            string maillist_global_firt_100 = "";
            //chargement de la liste des contacts qui vont recevoir les email pour les anniversaire sauf pour les derniers voeux


            foreach (Jesuite j in db.Jesuites)
            {
                jesuites.Add(j);
                //maillist = maillist + "," + j.Email;

            }


            //chargement de la liste de ceux qui vont recevoir le message anniv
            foreach (Jesuite j in jesuites_for_email)
            {

                maillist_global_firt_100 = maillist_global_firt_100 + "," + j.Email;
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

                    messageAnniv_singu.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));
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

                if (je.EntreeNoviciat.HasValue)
                {
                    if (je.EntreeNoviciat.Value.Month.Equals(DateTime.Now.Month) && je.EntreeNoviciat.Value.Day.Equals(DateTime.Now.Day) && ((DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 25) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 30) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 35) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 40) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 45) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 50) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 55) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 60) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 65) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 70) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 75) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 80) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 85) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 90)))
                    {
                        list_des_jubileviereligeuse.Add(je);
                    }
                }

                if (je.Ordination.HasValue)
                {
                    if (je.Ordination.Value.Month.Equals(DateTime.Now.Month) && je.Ordination.Value.Day.Equals(DateTime.Now.Day) && ((DateTime.Now.Year - je.Ordination.Value.Year == 25) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 30) || (DateTime.Now.Year - je.Ordination.Value.Year == 35) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 40) || (DateTime.Now.Year - je.Ordination.Value.Year == 45) || (DateTime.Now.Year - je.Ordination.Value.Year == 50) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 55) || (DateTime.Now.Year - je.Ordination.Value.Year == 60) || (DateTime.Now.Year - je.Ordination.Value.Year == 65) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 70) || (DateTime.Now.Year - je.Ordination.Value.Year == 75) || (DateTime.Now.Year - je.Ordination.Value.Year == 80) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 85) || (DateTime.Now.Year - je.Ordination.Value.Year == 90)))
                    {
                        list_des_jubilesacerdoce.Add(je);
                    }
                }


                if (je.DernierVoeux.HasValue)
                {
                    if (je.DernierVoeux.Value.Month.Equals(DateTime.Now.Month) && je.DernierVoeux.Value.Day.Equals(DateTime.Now.Day))
                    {
                        list_des_jubilederniervoeux.Add(je);
                    }
                }


            }
            // List<Jesuite> b = jesuites_for_email.Count();
            int a = 2;

            //Envoie des emails pour les anniversaires
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                // fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivNaissance == 1)
                {
                    //vérification de l'activation de l'envoie de message d'anniv de naissance
                    if (anniversereux.Count() == 1)
                    {

                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper();
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }
                       
                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");


                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Anniversaire du compagnon " + anniversereux.FirstOrDefault().NomComplete;
                            string Body = messageAnniv_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                              + anniversereux.FirstOrDefault().NomComplete.ToUpper() + "\r\n " + anniversereux.FirstOrDefault().Email + " "
                                                                    + messageAnniv_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "xbgbclhcwnpspwto");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }

                        }



                    }



                    //    //envoie de mail anniv pluriel
                    if (anniversereux.Count() > 1)
                    {


                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper() + ", ";
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }

                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");



                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Anniversaire des compagnons " + nomDesAnniversereux;
                            string Body = messageAnniv_pluri.FirstOrDefault().
                               Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                               + nomDesAnniversereuxAvecEmail + "\r\n"
                                                                     + messageAnniv_pluri.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "xbgbclhcwnpspwto");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }

                        }




                    }



                }


            }




            //Envoie des emails pour jubilé vie religieuse
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivVieReligieuse == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubileviereligeuse.Count() == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubileviereligeuse.FirstOrDefault().EntreeNoviciat.Value.Year;


                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper();
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }
                       

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");


                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de la vie religieuse du compagnon " + list_des_jubileviereligeuse.FirstOrDefault().NomComplete;
                            string Body = messageJubileVieReligieuse_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de vie réligieuse de " +
                              list_des_jubileviereligeuse.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubileviereligeuse.FirstOrDefault().Email + ") "
                                                                    + messageJubileVieReligieuse_singu.FirstOrDefault().CorpsMessage;


                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }
                    }


                    //envoie de mail jubilé pluriel
                    if (list_des_jubileviereligeuse.Count() > 1)
                    {


                        string nomDesjubileVieReligieuse = "";
                        string nomSansEmailDesjubileVieReligieuse = "";

                        foreach (Jesuite j in list_des_jubileviereligeuse)
                        {

                            nomDesjubileVieReligieuse = nomDesjubileVieReligieuse + "" + ", " + j.NomComplete.ToUpper() + " " + (DateTime.Now.Year - j.EntreeNoviciat.Value.Year) + " " + " ans de vie réligieuse" + " " + "( " + j.Email + ")";
                            nomSansEmailDesjubileVieReligieuse = nomSansEmailDesjubileVieReligieuse + "" + ", " + j.NomComplete;

                        }
                        
                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");
                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de la vie religieuse des compagnons " + nomSansEmailDesjubileVieReligieuse;
                            string Body = messageJubileVieReligieuse_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les jubilés de la vie réligieuse des compagnons " +
                              nomDesjubileVieReligieuse + ". " + messageJubileVieReligieuse_pluri.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }

                    }



                }


            }



            //Envoie des emails pour jubilé vie sacerdoce
            //verification du statut de la plateforme: en marche ou stop
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //vérification du statut de anniv sacerdoce
                if (db.Settings.FirstOrDefault().AnnivSacerdoce == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilesacerdoce.Count() == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;



                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de sacerdoce du compagnon " + list_des_jubilesacerdoce.FirstOrDefault().NomComplete;
                            string Body = messageJubilesacerdoce_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de sacerdoce de " +
                              list_des_jubilesacerdoce.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubilesacerdoce.FirstOrDefault().Email + ") "
                                                                    + messageJubilesacerdoce_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }




                    }

                    //envoie de mail jubilé pluriel
                    if (list_des_jubilesacerdoce.Count() > 1)
                    {

                        // int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;




                        string nomDesjubilesacerdoce = "";
                        string nomSansEmailDesjubileSacerdoce = "";

                        foreach (Jesuite j in list_des_jubilesacerdoce)
                        {

                            nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + j.NomComplete.ToUpper() + " " + (DateTime.Now.Year - j.Ordination.Value.Year) + " " + " ans de sacerdoce" + " " + "( " + j.Email + ")";
                            nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + j.NomComplete;

                        }

                       

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de sacerdoce des compagnons " + nomSansEmailDesjubileSacerdoce;
                            string Body = messageJubilesacerdoce_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les jubilés de sacerdoce des compagnons" + " " +
                              nomDesjubilesacerdoce + ". " + messageJubilesacerdoce_pluri.FirstOrDefault().CorpsMessage;


                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }


                    }


                }


            }


            //Envoie des emails pour jubilé derniers voeux
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivDernierVoeux == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilederniervoeux.Count() > 0)
                    {

                        foreach (Jesuite j in list_des_jubilederniervoeux)
                        {
                            //recupération du nombre d'année dernier voeux
                            int annee = DateTime.Now.Year - j.DernierVoeux.Value.Year;
                            //using (MailMessage mail = new MailMessage())



                            string nomDesjubilesacerdoce = "";
                            string nomSansEmailDesjubileSacerdoce = "";

                            foreach (Jesuite je in list_des_jubilesacerdoce)
                            {

                                nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + je.NomComplete.ToUpper() + " ( " + je.Email + " )";
                                nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + je.NomComplete;

                            }

                           
                            using (MailMessage ms = new MailMessage())
                            {
                                ms.From = new MailAddress("anniversairepao@gmail.com");

                                ms.To.Add(j.Email);

                                 ms.To.Add("naouful@yahoo.fr");
                                ms.Subject = "Rappel de tes derniers voeux";
                                string Body = messageRappelDernierVoeux_singu.FirstOrDefault().
                                  Titre + " \r\n " + ". Aujourd'hui " + " " + DateTime.Now.ToLongDateString().ToString() + " marque l'anniversaire de tes derniers voeux. Cela fait déjà " + annee + " année depuis votre incorporation définitive dans la Compagnie de Jésus " +
                                   "\r\n" + messageRappelDernierVoeux_singu.FirstOrDefault().CorpsMessage;

                                ms.AlternateViews.Add(Mail_Body(Body));
                                ms.IsBodyHtml = true;
                                ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                                {
                                    smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                    //smtpClient.Port = 25;
                                    smtpClient.EnableSsl = true;
                                    smtpClient.Send(ms);
                                }
                            }


                        }

                    }


                }


            }

            return 200;
        }


        public int SendFirst300()
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

            //liste brute des sj
            List<Jesuite> jesuites_for_email__1 = db.Jesuites.ToList();

            //liste 100 premiers 
            List<Jesuite> jesuites_for_email = new List<Jesuite>();

            jesuites_for_email__1.OrderByDescending(x => x.Nom);

            if (jesuites_for_email__1.Count() >= 130 )
            {
                for (int i = 131; i <= 200; i++)
                {
                    jesuites_for_email.Add(jesuites_for_email__1.ElementAt(i));
                }
            }
            



            string maillist_dernier_voeux = "";
            string maillist_global_firt_100 = "";
            //chargement de la liste des contacts qui vont recevoir les email pour les anniversaire sauf pour les derniers voeux


            foreach (Jesuite j in db.Jesuites)
            {
                jesuites.Add(j);
                //maillist = maillist + "," + j.Email;

            }


            //chargement de la liste de ceux qui vont recevoir le message anniv
            foreach (Jesuite j in jesuites_for_email)
            {

                maillist_global_firt_100 = maillist_global_firt_100 + "," + j.Email;
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

                    messageAnniv_singu.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));
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

                if (je.EntreeNoviciat.HasValue)
                {
                    if (je.EntreeNoviciat.Value.Month.Equals(DateTime.Now.Month) && je.EntreeNoviciat.Value.Day.Equals(DateTime.Now.Day) && ((DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 25) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 30) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 35) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 40) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 45) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 50) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 55) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 60) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 65) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 70) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 75) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 80) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 85) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 90)))
                    {
                        list_des_jubileviereligeuse.Add(je);
                    }
                }

                if (je.Ordination.HasValue)
                {
                    if (je.Ordination.Value.Month.Equals(DateTime.Now.Month) && je.Ordination.Value.Day.Equals(DateTime.Now.Day) && ((DateTime.Now.Year - je.Ordination.Value.Year == 25) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 30) || (DateTime.Now.Year - je.Ordination.Value.Year == 35) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 40) || (DateTime.Now.Year - je.Ordination.Value.Year == 45) || (DateTime.Now.Year - je.Ordination.Value.Year == 50) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 55) || (DateTime.Now.Year - je.Ordination.Value.Year == 60) || (DateTime.Now.Year - je.Ordination.Value.Year == 65) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 70) || (DateTime.Now.Year - je.Ordination.Value.Year == 75) || (DateTime.Now.Year - je.Ordination.Value.Year == 80) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 85) || (DateTime.Now.Year - je.Ordination.Value.Year == 90)))
                    {
                        list_des_jubilesacerdoce.Add(je);
                    }
                }


                if (je.DernierVoeux.HasValue)
                {
                    if (je.DernierVoeux.Value.Month.Equals(DateTime.Now.Month) && je.DernierVoeux.Value.Day.Equals(DateTime.Now.Day))
                    {
                        list_des_jubilederniervoeux.Add(je);
                    }
                }


            }
            // List<Jesuite> b = jesuites_for_email.Count();
            int a = 2;

            //Envoie des emails pour les anniversaires
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                // fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivNaissance == 1)
                {
                    //vérification de l'activation de l'envoie de message d'anniv de naissance
                    if (anniversereux.Count() == 1)
                    {

                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper();
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }
                      
                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");


                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Anniversaire du compagnon " + anniversereux.FirstOrDefault().NomComplete;
                            string Body = messageAnniv_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                              + anniversereux.FirstOrDefault().NomComplete.ToUpper() + "\r\n " + anniversereux.FirstOrDefault().Email + " "
                                                                    + messageAnniv_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "xbgbclhcwnpspwto");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }

                        }



                    }



                    //    //envoie de mail anniv pluriel
                    if (anniversereux.Count() > 1)
                    {


                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper() + ", ";
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }

                       

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");



                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Anniversaire des compagnons " + nomDesAnniversereux;
                            string Body = messageAnniv_pluri.FirstOrDefault().
                               Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                               + nomDesAnniversereuxAvecEmail + "\r\n"
                                                                     + messageAnniv_pluri.FirstOrDefault().CorpsMessage;
                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "xbgbclhcwnpspwto");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }

                        }




                    }



                }


            }




            //Envoie des emails pour jubilé vie religieuse
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivVieReligieuse == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubileviereligeuse.Count() == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubileviereligeuse.FirstOrDefault().EntreeNoviciat.Value.Year;


                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper();
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }
                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");


                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de la vie religieuse du compagnon " + list_des_jubileviereligeuse.FirstOrDefault().NomComplete;
                            string Body = messageJubileVieReligieuse_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de vie réligieuse de " +
                              list_des_jubileviereligeuse.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubileviereligeuse.FirstOrDefault().Email + ") "
                                                                    + messageJubileVieReligieuse_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }
                    }


                    //envoie de mail jubilé pluriel
                    if (list_des_jubileviereligeuse.Count() > 1)
                    {


                        string nomDesjubileVieReligieuse = "";
                        string nomSansEmailDesjubileVieReligieuse = "";

                        foreach (Jesuite j in list_des_jubileviereligeuse)
                        {

                            nomDesjubileVieReligieuse = nomDesjubileVieReligieuse + "" + ", " + j.NomComplete.ToUpper() + " " + (DateTime.Now.Year - j.EntreeNoviciat.Value.Year) + " " + " ans de vie réligieuse" + " " + "( " + j.Email + ")";
                            nomSansEmailDesjubileVieReligieuse = nomSansEmailDesjubileVieReligieuse + "" + ", " + j.NomComplete;

                        }
                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");
                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de la vie religieuse des compagnons " + nomSansEmailDesjubileVieReligieuse;
                            string Body = messageJubileVieReligieuse_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les jubilés de la vie réligieuse des compagnons " +
                              nomDesjubileVieReligieuse + ". " + messageJubileVieReligieuse_pluri.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }

                    }



                }


            }



            //Envoie des emails pour jubilé vie sacerdoce
            //verification du statut de la plateforme: en marche ou stop
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //vérification du statut de anniv sacerdoce
                if (db.Settings.FirstOrDefault().AnnivSacerdoce == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilesacerdoce.Count() == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;




                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de sacerdoce du compagnon " + list_des_jubilesacerdoce.FirstOrDefault().NomComplete;
                            string Body = messageJubilesacerdoce_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de sacerdoce de " +
                              list_des_jubilesacerdoce.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubilesacerdoce.FirstOrDefault().Email + ") "
                                                                    + messageJubilesacerdoce_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }




                    }

                    //envoie de mail jubilé pluriel
                    if (list_des_jubilesacerdoce.Count() > 1)
                    {

                        // int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;




                        string nomDesjubilesacerdoce = "";
                        string nomSansEmailDesjubileSacerdoce = "";

                        foreach (Jesuite j in list_des_jubilesacerdoce)
                        {

                            nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + j.NomComplete.ToUpper() + " " + (DateTime.Now.Year - j.Ordination.Value.Year) + " " + " ans de sacerdoce" + " " + "( " + j.Email + ")";
                            nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + j.NomComplete;

                        }

                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de sacerdoce des compagnons " + nomSansEmailDesjubileSacerdoce;
                            string Body = messageJubilesacerdoce_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les jubilés de sacerdoce des compagnons" + " " +
                              nomDesjubilesacerdoce + ". " + messageJubilesacerdoce_pluri.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }


                    }


                }


            }


            //Envoie des emails pour jubilé derniers voeux
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivDernierVoeux == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilederniervoeux.Count() > 0)
                    {

                        foreach (Jesuite j in list_des_jubilederniervoeux)
                        {
                            //recupération du nombre d'année dernier voeux
                            int annee = DateTime.Now.Year - j.DernierVoeux.Value.Year;
                            //using (MailMessage mail = new MailMessage())



                            string nomDesjubilesacerdoce = "";
                            string nomSansEmailDesjubileSacerdoce = "";

                            foreach (Jesuite je in list_des_jubilesacerdoce)
                            {

                                nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + je.NomComplete.ToUpper() + " ( " + je.Email + " )";
                                nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + je.NomComplete;

                            }

                            
                            using (MailMessage ms = new MailMessage())
                            {
                                ms.From = new MailAddress("anniversairepao@gmail.com");

                                ms.To.Add(j.Email);

                                 ms.To.Add("naouful@yahoo.fr");
                                ms.Subject = "Rappel de tes derniers voeux";
                                string Body = messageRappelDernierVoeux_singu.FirstOrDefault().
                                  Titre + " \r\n " + ". Aujourd'hui " + " " + DateTime.Now.ToLongDateString().ToString() + " marque l'anniversaire de tes derniers voeux. Cela fait déjà " + annee + " année depuis votre incorporation définitive dans la Compagnie de Jésus " +
                                   "\r\n" + messageRappelDernierVoeux_singu.FirstOrDefault().CorpsMessage;


                                ms.AlternateViews.Add(Mail_Body(Body));
                                ms.IsBodyHtml = true;
                                ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                                {
                                    smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                    //smtpClient.Port = 25;
                                    smtpClient.EnableSsl = true;
                                    smtpClient.Send(ms);
                                }
                            }


                        }

                    }


                }


            }

            return 300;
        }


        public int SendFirst400()
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

            //liste brute des sj
            List<Jesuite> jesuites_for_email__1 = db.Jesuites.ToList();

            //liste des 100  
            List<Jesuite> jesuites_for_email = new List<Jesuite>();

            jesuites_for_email__1.OrderByDescending(x => x.Nom);

            if (jesuites_for_email__1.Count() >= 200 )
            {
                for (int i = 201; i <= jesuites_for_email__1.Count()-1; i++)
                {
                    jesuites_for_email.Add(jesuites_for_email__1.ElementAt(i));
                }
            }




            string maillist_dernier_voeux = "";
            string maillist_global_firt_100 = "";
            //chargement de la liste des contacts qui vont recevoir les email pour les anniversaire sauf pour les derniers voeux


            foreach (Jesuite j in db.Jesuites)
            {
                jesuites.Add(j);
                //maillist = maillist + "," + j.Email;

            }


            //chargement de la liste de ceux qui vont recevoir le message anniv
            foreach (Jesuite j in jesuites_for_email)
            {

                maillist_global_firt_100 = maillist_global_firt_100 + "," + j.Email;
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

                    messageAnniv_singu.Add(new messageModelView(me_an.Titre, me_an.CorpsMessage, me_an.CorpsMessage2, me_an.NombrePersonne.NombreAnniversereux, me_an.TypeMessage.Label));
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

                if (je.EntreeNoviciat.HasValue)
                {
                    if (je.EntreeNoviciat.Value.Month.Equals(DateTime.Now.Month) && je.EntreeNoviciat.Value.Day.Equals(DateTime.Now.Day) && ((DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 25) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 30) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 35) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 40) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 45) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 50) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 55) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 60) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 65) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 70) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 75) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 80) ||
                (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 85) || (DateTime.Now.Year - je.EntreeNoviciat.Value.Year == 90)))
                    {
                        list_des_jubileviereligeuse.Add(je);
                    }
                }

                if (je.Ordination.HasValue)
                {
                    if (je.Ordination.Value.Month.Equals(DateTime.Now.Month) && je.Ordination.Value.Day.Equals(DateTime.Now.Day) && ((DateTime.Now.Year - je.Ordination.Value.Year == 25) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 30) || (DateTime.Now.Year - je.Ordination.Value.Year == 35) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 40) || (DateTime.Now.Year - je.Ordination.Value.Year == 45) || (DateTime.Now.Year - je.Ordination.Value.Year == 50) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 55) || (DateTime.Now.Year - je.Ordination.Value.Year == 60) || (DateTime.Now.Year - je.Ordination.Value.Year == 65) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 70) || (DateTime.Now.Year - je.Ordination.Value.Year == 75) || (DateTime.Now.Year - je.Ordination.Value.Year == 80) ||
                (DateTime.Now.Year - je.Ordination.Value.Year == 85) || (DateTime.Now.Year - je.Ordination.Value.Year == 90)))
                    {
                        list_des_jubilesacerdoce.Add(je);
                    }
                }


                if (je.DernierVoeux.HasValue)
                {
                    if (je.DernierVoeux.Value.Month.Equals(DateTime.Now.Month) && je.DernierVoeux.Value.Day.Equals(DateTime.Now.Day))
                    {
                        list_des_jubilederniervoeux.Add(je);
                    }
                }


            }
            // List<Jesuite> b = jesuites_for_email.Count();
            int a = 2;

            //Envoie des emails pour les anniversaires
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                // fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivNaissance == 1)
                {
                    //vérification de l'activation de l'envoie de message d'anniv de naissance
                    if (anniversereux.Count() == 1)
                    {

                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper();
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }
                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");


                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Anniversaire du compagnon " + anniversereux.FirstOrDefault().NomComplete;
                            string Body = messageAnniv_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                              + anniversereux.FirstOrDefault().NomComplete.ToUpper() + "\r\n " + anniversereux.FirstOrDefault().Email + " "
                                                                    + messageAnniv_singu.FirstOrDefault().CorpsMessage;
                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "xbgbclhcwnpspwto");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }

                        }



                    }



                    //    //envoie de mail anniv pluriel
                    if (anniversereux.Count() > 1)
                    {


                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper() + ", "; ;
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }

                        

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");



                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Anniversaire des compagnons " + nomDesAnniversereux;
                            string Body = messageAnniv_pluri.FirstOrDefault().
                               Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString() + ", nous célébrons l'anniversaire de naissance de \r\n"
                               + nomDesAnniversereuxAvecEmail + "\r\n"
                                                                     + messageAnniv_pluri.FirstOrDefault().CorpsMessage;
                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "xbgbclhcwnpspwto");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }

                        }




                    }



                }


            }




            //Envoie des emails pour jubilé vie religieuse
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivVieReligieuse == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubileviereligeuse.Count() == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubileviereligeuse.FirstOrDefault().EntreeNoviciat.Value.Year;


                        string nomDesAnniversereux = "";
                        string nomDesAnniversereuxAvecEmail = "";

                        foreach (Jesuite j in anniversereux)
                        {

                            nomDesAnniversereux = nomDesAnniversereux + "" + j.NomComplete.ToUpper();
                            nomDesAnniversereuxAvecEmail = nomDesAnniversereuxAvecEmail + "" + ", " + j.NomComplete.ToUpper() + " ( " + j.Email + " )";
                        }
                       

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");


                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de la vie religieuse du compagnon " + list_des_jubileviereligeuse.FirstOrDefault().NomComplete;
                            string Body = messageJubileVieReligieuse_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de vie réligieuse de " +
                              list_des_jubileviereligeuse.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubileviereligeuse.FirstOrDefault().Email + ") "
                                                                    + messageJubileVieReligieuse_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }
                    }


                    //envoie de mail jubilé pluriel
                    if (list_des_jubileviereligeuse.Count() > 1)
                    {


                        string nomDesjubileVieReligieuse = "";
                        string nomSansEmailDesjubileVieReligieuse = "";

                        foreach (Jesuite j in list_des_jubileviereligeuse)
                        {

                            nomDesjubileVieReligieuse = nomDesjubileVieReligieuse + "" + ", " + j.NomComplete.ToUpper() + " " + (DateTime.Now.Year - j.EntreeNoviciat.Value.Year) + " " + " ans de vie réligieuse" + " " + "( " + j.Email + ")";
                            nomSansEmailDesjubileVieReligieuse = nomSansEmailDesjubileVieReligieuse + "" + ", " + j.NomComplete;

                        }
                       
                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");
                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de la vie religieuse des compagnons " + nomSansEmailDesjubileVieReligieuse;
                            string Body = messageJubileVieReligieuse_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les jubilés de la vie réligieuse des compagnons " +
                              nomDesjubileVieReligieuse + ". " + messageJubileVieReligieuse_pluri.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }

                    }



                }


            }



            //Envoie des emails pour jubilé vie sacerdoce
            //verification du statut de la plateforme: en marche ou stop
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //vérification du statut de anniv sacerdoce
                if (db.Settings.FirstOrDefault().AnnivSacerdoce == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilesacerdoce.Count() == 1)
                    {
                        //recupération du nombre d'année dans la vie religieuse
                        int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;




                       

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de sacerdoce du compagnon " + list_des_jubilesacerdoce.FirstOrDefault().NomComplete;
                            string Body = messageJubilesacerdoce_singu.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les " + annee + " ans de sacerdoce de " +
                              list_des_jubilesacerdoce.FirstOrDefault().NomComplete.ToUpper() + "\r\n (" + list_des_jubilesacerdoce.FirstOrDefault().Email + ") "
                                                                    + messageJubilesacerdoce_singu.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }




                    }

                    //envoie de mail jubilé pluriel
                    if (list_des_jubilesacerdoce.Count() > 1)
                    {

                        // int annee = DateTime.Now.Year - list_des_jubilesacerdoce.FirstOrDefault().Ordination.Value.Year;




                        string nomDesjubilesacerdoce = "";
                        string nomSansEmailDesjubileSacerdoce = "";

                        foreach (Jesuite j in list_des_jubilesacerdoce)
                        {

                            nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + j.NomComplete.ToUpper() + " " + (DateTime.Now.Year - j.Ordination.Value.Year) + " " + " ans de sacerdoce" + " " + "( " + j.Email + ")";
                            nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + j.NomComplete;

                        }

                       

                        using (MailMessage ms = new MailMessage())
                        {
                            ms.From = new MailAddress("anniversairepao@gmail.com");

                            ms.To.Add(maillist_global_firt_100);

                            ms.Subject = "Jubilé de sacerdoce des compagnons " + nomSansEmailDesjubileSacerdoce;
                            string Body = messageJubilesacerdoce_pluri.FirstOrDefault().
                              Titre + " \r\n " + "Aujourd'hui," + " " + DateTime.Now.ToLongDateString().ToString() + ", nous célébrons les jubilés de sacerdoce des compagnons" + " " +
                              nomDesjubilesacerdoce + ". " + messageJubilesacerdoce_pluri.FirstOrDefault().CorpsMessage;

                            ms.AlternateViews.Add(Mail_Body(Body));
                            ms.IsBodyHtml = true;
                            ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                //smtpClient.Port = 25;
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(ms);
                            }
                        }


                    }


                }


            }


            //Envoie des emails pour jubilé derniers voeux
            //verification si lheure de l'envoie est fixé
            if (db.Settings.FirstOrDefault().StopAll == 1)
            {

                //fixation de l'heure du début de l'envoie de email
                if (db.Settings.FirstOrDefault().AnnivDernierVoeux == 1)
                {
                    //vérification du nombre de ceux qui fete leur jubilé
                    if (list_des_jubilederniervoeux.Count() > 0)
                    {

                        foreach (Jesuite j in list_des_jubilederniervoeux)
                        {
                            //recupération du nombre d'année dernier voeux
                            int annee = DateTime.Now.Year - j.DernierVoeux.Value.Year;
                            //using (MailMessage mail = new MailMessage())


                            string nomDesjubilesacerdoce = "";
                            string nomSansEmailDesjubileSacerdoce = "";

                            foreach (Jesuite je in list_des_jubilesacerdoce)
                            {

                                nomDesjubilesacerdoce = nomDesjubilesacerdoce + "" + ", " + je.NomComplete.ToUpper() + " ( " + je.Email + " )";
                                nomSansEmailDesjubileSacerdoce = nomSansEmailDesjubileSacerdoce + "" + ", " + je.NomComplete;

                            }

                            
                            using (MailMessage ms = new MailMessage())
                            {
                                ms.From = new MailAddress("anniversairepao@gmail.com");

                                ms.To.Add(j.Email);

                                ms.To.Add("naouful@yahoo.fr");
                                ms.Subject = "Rappel de tes derniers voeux";
                                string Body = messageRappelDernierVoeux_singu.FirstOrDefault().
                                  Titre + " \r\n " + ". Aujourd'hui " + " " + DateTime.Now.ToLongDateString().ToString() + " marque l'anniversaire de tes derniers voeux. Cela fait déjà " + annee + " année depuis votre incorporation définitive dans la Compagnie de Jésus " +
                                   "\r\n" + messageRappelDernierVoeux_singu.FirstOrDefault().CorpsMessage;

                                ms.AlternateViews.Add(Mail_Body(Body));
                                ms.IsBodyHtml = true;
                                ms.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;


                                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                                {
                                    smtpClient.Credentials = new NetworkCredential("anniversairepao@gmail.com", "Province2021");
                                    //smtpClient.Port = 25;
                                    smtpClient.EnableSsl = true;
                                    smtpClient.Send(ms);
                                }
                            }


                        }

                    }


                }


            }

            return 400;
        }
    }
}