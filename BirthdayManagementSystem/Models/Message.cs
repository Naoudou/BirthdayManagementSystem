//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BirthdayManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int IdMessage { get; set; }
        public string Titre { get; set; }
        public string CorpsMessage { get; set; }
        public string CorpsMessage2 { get; set; }
        public int IdTypeMessage { get; set; }
        public Nullable<int> IdNombrePersonne { get; set; }
    
        public virtual NombrePersonne NombrePersonne { get; set; }
        public virtual TypeMessage TypeMessage { get; set; }
    }
}