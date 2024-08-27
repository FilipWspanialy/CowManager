using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CowManagerApp.MVC.Models
{
    public class CowTreatAdd
    {
        public int CowId { get; set; }
        public string CowName { get; set; }

        public int? DiagId { get; set; }

        [Required]
        public int SelectedMedicinetId { get; set; }
        public IEnumerable<Medicine> Medicines { get; set; }

        public string Comment { get; set; }
    }
}
