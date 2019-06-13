using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Flight_Low_fare_Search.Models
{
    public class InfoLet
    {
        [DisplayName("Odrediste")]
        public string arrival { get; set; }
        [DisplayName("Polaziste")]
        public string departure { get; set; }
        [DisplayName("Datum polaska")]
        public string datumPolaska { get; set; }
        [DisplayName("Datum dolaska")]
        public string datumDolaska { get; set; }
        [DisplayName("Broj slobodnih mjesta")]
        public int? brojSlobodnihMjesta { get; set; }
        [DisplayName("Cijena")]
        public decimal cijena { get; set; }
        [DisplayName("Valuta")]
        public string valuta { get; set; }


        [DisplayName("Dolazak na presjedanje")]
        public string polazakPresjedanje { get; set; }
        [DisplayName("Polazak sa presjedanja")]
        public string dolazakPresjedanje { get; set; }
        [DisplayName("Datum dolaska")]
        public string polazakPresjedanjeDatum { get; set; }
        [DisplayName("Datum polaska")]
        public string dolazakPresjedanjeDatum { get; set; }
        [DisplayName("Broj presjedanja")]
        public int brojPresjedanja { get; set; }

    }
}