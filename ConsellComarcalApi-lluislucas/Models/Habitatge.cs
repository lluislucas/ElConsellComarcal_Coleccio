namespace ConsellComarcalApi.Models;

public class Habitatge
{
    public Habitatge(string identificador, string adreca, string poblacio, string codiPostal, TipusImmoble tipusImmoble, int metresQuadrats, int habitantsImmoble, int numMenorsImmoble, string? nomContribuent, string? dNIContribuent)
    {
        Identificador = identificador;
        Adreca = adreca;
        Poblacio = poblacio;
        CodiPostal = codiPostal;
        TipusImmoble = tipusImmoble;
        MetresQuadrats = metresQuadrats;
        HabitantsImmoble = habitantsImmoble;
        NumMenorsImmoble = numMenorsImmoble;
        NomContribuent = nomContribuent;
        DNIContribuent = dNIContribuent;
        
    }

    public string Identificador {get; set;}
    public string Adreca {get; set;}
    public string Poblacio {get; set;}
    public string CodiPostal {get; set;}
    public TipusImmoble TipusImmoble {get; set;}
    public int MetresQuadrats {get; set;}
    public int HabitantsImmoble {get; set;}
    public int NumMenorsImmoble {get; set;}
    public string? NomContribuent {get; set;}
    public string? DNIContribuent {get; set;}
    public bool EsFamiliaNumerosa 
    {
        get { return HabitantsImmoble > 5; }
    } 
    // public bool EsFamiliaNumerosa => HabitantsImmoble >= 5;


}