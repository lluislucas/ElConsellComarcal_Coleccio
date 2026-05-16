using ConsellComarcalElRetorn.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConsellComarcalElRetorn.Pages;

public class TaxaContribuent : PageModel
{
    public IHttpClientFactory _clientFactory;
    
    public string NomContribuent { get; set; }
    public string Població { get; set; }
    public double QuotaFinal { get; set; }
    public int Casa { get; set; }
    public int Pis { get; set; }
    public int Terreny { get; set; }
    public bool Mes5persones { get; set; }
    public HabitatgeDades Habitatges1 {get;set;}

    public TaxaContribuent(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    public List<HabitatgeDades> Habitatges { get; set; } = new();
    public async Task OnGet(string poblacio, string dni)
    {
        Població = poblacio;
        var client = _clientFactory.CreateClient();
        var llistaHabitatges = await client.GetFromJsonAsync<List<HabitatgeDades>>($"http://localhost:5000/habitatges/{dni}");

        if (llistaHabitatges == null) return;

        Habitatges = llistaHabitatges;
        //CASA
        var casa = 0;
        int MetresCasa = 0;
        double calculCasa = 0;

        //PIS
        var pis = 0;
        var MetresPis = 0;
        double calculPis = 0;

        //TERRENY
        var terreny = 0;
        int MetresTerreny = 0;
        double calculTerreny = 0;

        double totalPersonesAquestaCiutat = 0;

        bool MesDe5Persones = false;
        bool MenorsEdat = false;
        bool DescomptePobleTrampos = false;

        foreach (var h in Habitatges)
        {
            if (h.Població == Població)
            {
                NomContribuent = h.NomPagaTaxa;
                totalPersonesAquestaCiutat += (h.QuantitatPersonesQueViuenMajors +
                                               h.QuantitatPersonesQueViuenMenors);

                if (totalPersonesAquestaCiutat >= 5) MesDe5Persones = true;

                switch (h.TipusImmoble)
                {
                    case "Pis":
                        pis++;
                        calculPis += h.MetresQuadrats * 0.996;
                        MetresPis += h.MetresQuadrats;
                        break;

                    case "Casa":
                        casa++;
                        MetresCasa += h.MetresQuadrats;
                        calculCasa += h.MetresQuadrats * 0.998;
                        if (h.QuantitatPersonesQueViuenMenors > 0) calculCasa *= 1.05;
                        break;

                    case "Terreny":
                        terreny++;
                        calculTerreny += h.MetresQuadrats * 0.136;
                        MetresTerreny += h.MetresQuadrats;
                        break;
                }

                if (h.CodiPostal == 17970 || h.CodiPostal == 17971) DescomptePobleTrampos = true;
            }
        }

        double QuotaTotal = calculCasa + calculPis + calculTerreny;

        if (MesDe5Persones == true) QuotaTotal *= 0.90;

        if (DescomptePobleTrampos == true) QuotaTotal *= 0.75;

        QuotaFinal = Math.Round(QuotaTotal, 2);
        Casa = casa;
        Pis = pis;
        Terreny = terreny;
        Mes5persones= MesDe5Persones;
    }
}