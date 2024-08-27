using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CowManagerApp.MVC.Models
{
    public class CowDiagAdd
    {
        public int CowId { get; set; }
        public string CowName { get; set; }

        [Required]
        public int SelectedDiseaseId { get; set; }
        public IEnumerable<Disease> Diseases { get; set; }

        public string Comment { get; set; }
    }
}
