namespace ConsellComarcalElRetorn.Models;

public class HabitatgeDades
{ 
    public string Identificador {get; set; }
    public string Adreça {get;set;}
    public string Població {get;set;}
    public int CodiPostal {get;set;}
    public string TipusImmoble {get;set;}
    public int MetresQuadrats{get;set;}
    public int QuantitatPersonesQueViuenMajors {get;set;}
    public int QuantitatPersonesQueViuenMenors {get;set;}
    public string NomPagaTaxa {get;set;}
    public string DNIPagaTaxa {get;set;}
}