namespace ConsellComarcalApi.Models;

using System.ComponentModel.DataAnnotations.Schema;

[Table("edificacions")]
public class Edificacio
{
    public int Id { get; set; }
    public string Direccio { get; set; }

    [Column("poble_id")]
    public int PobleId { get; set; }

    [Column("contribuent_id")]
    public int ContribuentId { get; set; }

    [Column("tipus_id")]
    public int TipusId { get; set; }

    public int Habitants { get; set; }
    public int Menors { get; set; }

    [Column("metres_quadrats")]
    public int MetresQuadrats { get; set; }

    public Poble Poble { get; set; }
    public Contribuent Contribuent { get; set; }
    public Tipus Tipus { get; set; }
}