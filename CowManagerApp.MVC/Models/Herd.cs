using System;
using System.Collections.Generic;

namespace CowManagerApp.MVC.Models;

public partial class Herd
{
    public int Id { get; set; }

    public int AmountOfCows { get; set; }

    public string? Comment { get; set; }

    public virtual ICollection<Cow> Cows { get; set; } = new List<Cow>();
}
