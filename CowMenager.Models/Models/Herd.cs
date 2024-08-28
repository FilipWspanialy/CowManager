using System;
using System.Collections.Generic;

namespace CowManager.Models.Models;

public partial class Herd
{
    public int Id { get; set; }

    public int AmountOfCows => Cows?.Count ?? 0;

    public string? Comment { get; set; }

    public virtual ICollection<Cow> Cows { get; set; } = new List<Cow>();
}
