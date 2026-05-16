using ConsellComarcalElRetorn.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConsellComarcalElRetorn.Pages;

public class Contribuents : PageModel
{
    public IHttpClientFactory _clientFactory;

    public List<DadesPersones> DadesContribuents { get; set; }
    public string Població { get; set; }
    public Contribuents(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task OnGet(string població)
    {
        Població = població;
        var client = _clientFactory.CreateClient();
        var resultat = await client.GetFromJsonAsync<List<DadesPersones>>($"http://localhost:5000/contribuent/{població}");
        
        if (resultat == null)
        {
            throw new Exception("El poble no s'ha trobat");
        }
        
        DadesContribuents = resultat;
    }
}