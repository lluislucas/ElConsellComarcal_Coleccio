namespace ConsellComarcalApi.Models;

public class Contribuent
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Dni { get; set; }

    public ICollection<Edificacio> Edificacions { get; set; }
}