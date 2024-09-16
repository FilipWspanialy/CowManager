using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CowManager.Models.Models
{
    public class CowDocumentation
    {
        public Cow Cow { get; set; }
        public List<Treatment> Treatments { get; set; }

        public List<Diagnosis> Diagnosis { get; set; }
    }
}
