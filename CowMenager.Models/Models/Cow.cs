using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CowManager.Models.Models;


public partial class Cow
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters and spaces")]
    public string? Name { get; set; }

    public int? Idherd { get; set; }

    public string? Comment { get; set; }

    public virtual Herd? IdherdNavigation { get; set; }
}
