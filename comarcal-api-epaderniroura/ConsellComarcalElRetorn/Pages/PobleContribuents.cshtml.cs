using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConsellComarcalElRetorn.Pages;

public class PobleContribuents: PageModel
{
    public IHttpClientFactory _clientFactory;
    
    public List<string> LlistaPobles { get; set; } = new();
    
    public PobleContribuents(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    public async Task OnGet()
    {
        var client = _clientFactory.CreateClient();
        var resultat = await client.GetFromJsonAsync<List<string>>($"http://localhost:5000/poble");
        if (resultat == null) 
        {
            throw new Exception("No s'ha trobat el poble");
        }

        LlistaPobles = resultat;
    }
}