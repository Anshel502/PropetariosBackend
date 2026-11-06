using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Beer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public int BeerId {get; set;}
public string Name {get; set;} = string.Empty;
public int BrandId {get; set;}
[Column(TypeName ="decimal(18,2)")]
public decimal Alcohol {get; set;}
[ForeignKey("BrandId")]
public virtual Brand Brand {get; set;} = null!;
}
