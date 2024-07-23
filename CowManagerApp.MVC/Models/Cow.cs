using System;
using System.Collections.Generic;

namespace CowManagerApp.MVC.Models;

public partial class Cow
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Idherd { get; set; }

    public string? Comment { get; set; }

    public virtual Herd? IdherdNavigation { get; set; }
}
