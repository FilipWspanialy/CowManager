using System;
using System.Collections.Generic;

namespace CowManager.Models.Models;

public partial class Disease
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Comment { get; set; }
}
