using System;
using System.Collections.Generic;


namespace CowManager.Models.Models
{
    public class CowTreat
    {
        public Cow Cow { get; set; }
        public List<Treatment> Treatments { get; set; }
    }
}


