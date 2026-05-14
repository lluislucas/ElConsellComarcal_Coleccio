namespace ConsellComarcalApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

[Table("tipus")]
public class Tipus
{
    public int Id { get; set; }
    public string Nom { get; set; }

    public ICollection<Edificacio> Edificacions { get; set; }
}