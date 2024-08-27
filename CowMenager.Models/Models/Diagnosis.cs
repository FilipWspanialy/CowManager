using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CowManagerApp.MVC.Models;

public partial class Diagnosis
{
    [Key]
    public int Id { get; set; }

    public int Idcow { get; set; }

    public int Iddisease { get; set; }

    public string NameOfDisease { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual Cow IdcowNavigation { get; set; } = null!;

    public virtual Disease IddiseaseNavigation { get; set; } = null!;
}
