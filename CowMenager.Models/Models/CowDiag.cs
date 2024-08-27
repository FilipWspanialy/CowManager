using System;
using System.Collections.Generic;


namespace CowManagerApp.MVC.Models
{
    public class CowDiag
    {
        public Cow Cow { get; set; }
        public List<Diagnosis> Diagnoses { get; set; }
    }
}


