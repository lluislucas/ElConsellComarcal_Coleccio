using Microsoft.AspNetCore.Mvc.RazorPages;
using ConsellComarcalElRetorn.Models;

namespace ConsellComarcalElRetorn.Pages;
public class Habitatge : PageModel
{
    public IHttpClientFactory _clientFactory;

    public List<HabitatgeDades> Dades {get;set;}
    public string Població { get; set; }

    public Habitatge(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task OnGet(string població)
    {
        Població = població;
        var client = _clientFactory.CreateClient();
        var resultat = await client.GetFromJsonAsync<List<HabitatgeDades>>($"http://localhost:5000/habitants/{Població}");
        
        if (resultat == null) 
        {
            throw new Exception("No s'ha trobat l'habitatge");
        }
        
        Dades = resultat;
    }
}