using System;
using System.Collections.Generic;

namespace CowManagerApp.MVC.Models;

public partial class Diagnosis
{
    public int Idcow { get; set; }

    public int Iddisease { get; set; }

    public string NameOfDisease { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual Cow IdcowNavigation { get; set; } = null!;

    public virtual Disease IddiseaseNavigation { get; set; } = null!;
}
