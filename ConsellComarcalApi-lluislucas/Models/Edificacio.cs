namespace ConsellComarcalApi.Models;

public class Edificacio
{
    public int Id { get; set; }
    public string Direccio { get; set; }
    public int PobleId { get; set; }
    public int ContribuentId { get; set; }
    public int TipusId { get; set; }
    public int Habitants { get; set; }
    public int Menors { get; set; }
    public int MetresQuadrats { get; set; }

    public Poble Poble { get; set; }
    public Contribuent Contribuent { get; set; }
    public Tipus Tipus { get; set; }
}