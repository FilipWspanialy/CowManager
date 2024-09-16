using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CowManager.Models.Models;

public partial class Treatment
{
    [Key]
    public int Id { get; set; }

    public int Idcow { get; set; }

    public int Idmedicine { get; set; }

    public string NameOfMedicine { get; set; } = null!;

    public int? Iddiagnosis { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? DeleteTime { get; set; }

    public virtual Cow IdcowNavigation { get; set; } = null!;

    public virtual Medicine IdmedicineNavigation { get; set; } = null!;

    public virtual Diagnosis IddiagnosisNavigation { get; set; } = null!;
}

