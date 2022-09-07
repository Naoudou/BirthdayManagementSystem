using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BirthdayManagementSystem.Models
{
    public partial class Setting
    {
        public Setting()
        {
        }

        public Setting(int idSetting, int? annivNaissance, int? annivVieReligieuse, int? annivSacerdoce, int? annivDernierVoeux, int? set1, int? set2, DateTime? heureDenvoie, DateTime? heureDeReset, int? stopAll)
        {
            IdSetting = idSetting;
            AnnivNaissance = annivNaissance;
            AnnivVieReligieuse = annivVieReligieuse;
            AnnivSacerdoce = annivSacerdoce;
            AnnivDernierVoeux = annivDernierVoeux;
            Set1 = set1;
            Set2 = set2;
            HeureDenvoie = heureDenvoie;
            HeureDeReset = heureDeReset;
            StopAll = stopAll;
        }

        public int IdSetting { get; set; }
        public Nullable<int> AnnivNaissance { get; set; }
        public Nullable<int> AnnivVieReligieuse { get; set; }
        public Nullable<int> AnnivSacerdoce { get; set; }
        public Nullable<int> AnnivDernierVoeux { get; set; }
        public Nullable<int> Set1 { get; set; }
        public Nullable<int> Set2 { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public Nullable<System.DateTime> HeureDenvoie { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public Nullable<System.DateTime> HeureDeReset { get; set; }
        public Nullable<int> StopAll { get; set; }
    }
}