using System;
using System.Collections.Generic;

namespace CowManager.Models.Models;

public partial class Medicine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Comment { get; set; }
}
