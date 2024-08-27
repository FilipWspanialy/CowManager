
using System;
using System.Collections.Generic;


namespace CowManagerApp.MVC.Models
{
    public class CowTreat
    {
        public Cow Cow { get; set; }
        public List<Treatment> Treatments { get; set; }
    }
}


