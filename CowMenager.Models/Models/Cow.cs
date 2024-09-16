using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CowManager.Models.Models;


public partial class Cow
{
    public int Id { get; set; }

    [RegularExpression(@"^\d{6}$", ErrorMessage = "Cowid must be exactly 6 digits.")]
    public string Cowid { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters and spaces")]
    public string? Name { get; set; }

    public int? Idherd { get; set; }

    public string? Nameherd { get; set; }

    public string? Comment { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public bool IsInactive { get; set; }

    public virtual Herd? IdherdNavigation { get; set; }
}
