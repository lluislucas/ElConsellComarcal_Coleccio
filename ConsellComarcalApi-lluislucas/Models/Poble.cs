namespace ConsellComarcalApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

[Table("pobles")]
public class Poble
{
    public int Id { get; set; }
    public string Nom { get; set; }

    [Column("codi_postal")]
    public string CodiPostal { get; set; }
}