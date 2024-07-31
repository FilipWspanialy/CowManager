using System;
using System.Collections.Generic;

namespace CowManagerApp.MVC.Models
{
    public partial class Condition
    {
        public IEnumerable<Disease>? Diseases { get; set; }
        public IEnumerable<Medicine>? Medicines { get; set; }
    }

}
