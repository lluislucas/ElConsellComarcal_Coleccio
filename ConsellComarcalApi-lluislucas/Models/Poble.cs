public class Poble
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string CodiPostal { get; set; }

    public ICollection<Edificacio> Edificacions { get; set; }
}