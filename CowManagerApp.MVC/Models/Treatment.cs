using System;
using System.Collections.Generic;

namespace CowManagerApp.MVC.Models;

public partial class Treatment
{
    public int Idcow { get; set; }

    public int Idmedicine { get; set; }

    public string NameOfMedicine { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual Cow IdcowNavigation { get; set; } = null!;

    public virtual Medicine IdmedicineNavigation { get; set; } = null!;
}
