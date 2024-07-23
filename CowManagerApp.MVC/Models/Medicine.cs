using System;
using System.Collections.Generic;

namespace CowManagerApp.MVC.Models;

public partial class Medicine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Comment { get; set; }
}
