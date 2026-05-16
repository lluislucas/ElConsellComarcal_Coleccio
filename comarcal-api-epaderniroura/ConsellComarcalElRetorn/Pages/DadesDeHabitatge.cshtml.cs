using ConsellComarcalElRetorn.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConsellComarcalElRetorn.Pages;

public class DadesDeHabitatge : PageModel
{
    public IHttpClientFactory _clientFactory;
    public  string Identificador { get; set; }
    public HabitatgeDades LlistaPobles { get; set; } = new();
    
    public DadesDeHabitatge(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task OnGet(string Identificador)
    {
        var client = _clientFactory.CreateClient();
        var resultat = await client.GetFromJsonAsync<HabitatgeDades>($"http://localhost:5000/habitant/{Identificador}");
        
        LlistaPobles = resultat;
    }
}