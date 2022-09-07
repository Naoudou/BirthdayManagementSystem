using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BirthdayManagementSystem.Models
{
    public class messageModelView
    {
        public messageModelView(string titre, string corpsMessage, string corpsMessage2, int nombrePersonne, string typeMessage)
        {
            Titre = titre;
            CorpsMessage = corpsMessage;
            CorpsMessage2 = corpsMessage2;
            NombrePersonne = nombrePersonne;
            TypeMessage = typeMessage;
        }

        public messageModelView()
        {
        }

        public string Titre { get; set; }
        public string CorpsMessage { get; set; }
        public string CorpsMessage2 { get; set; }
        public int NombrePersonne { get; set; }
        public string TypeMessage { get; set; }


    }
}