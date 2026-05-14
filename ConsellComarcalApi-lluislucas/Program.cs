using ConsellComarcalApi.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

using var db = new AppDbContext();

var registres = db.Edificacions
    .Include(e => e.Poble)
    .Include(e => e.Contribuent)
    .Include(e => e.Tipus)
    .Select(e=> new
    {
        Identificador = e.Id,
        Adreca = e.Direccio,
        Poblacio = e.Poble.Nom,
        CodiPostal = e.Poble.CodiPostal,
        TipusImmoble = e.Tipus.Id-1,
        MetresQuadrats = e.MetresQuadrats,
        HabitantsImmoble = e.Habitants,
        NumMenorsImmoble = e.Menors,
        NomContribuent = e.Contribuent.Nom,
        DNIContribuent = e.Contribuent.Dni,
        EsFamiliaNumerosa = e.Habitants >5

    }
).ToList();

List<Habitatge> habitatges = new();
foreach (var r in registres)
{
    Habitatge h = new(
        r.Identificador.ToString(),
        r.Adreca,
        r.Poblacio,
        r.CodiPostal,
        (TipusImmoble)r.TipusImmoble,
        r.MetresQuadrats,
        r.HabitantsImmoble,
        r.NumMenorsImmoble,
        r.NomContribuent,
        r.DNIContribuent
    );
    habitatges.Add(h);
}
// Console.WriteLine(todas.Count);

// foreach(var t in todas)
// {
//     Console.WriteLine(JsonSerializer.Serialize(t, new JsonSerializerOptions {WriteIndented = true}));
// }







// List<Habitatge> habitatges = new List<Habitatge>
// {

//     // Joan Pujol (12345678A) - té habitatges a Figueres i Roses (per comprovar que no barreja pobles)
//     new Habitatge("1", "Carrer Major 12", "Figueres", "17600", TipusImmoble.Pis, 90, 3, 1, "Joan Pujol", "12345678A"),
//     new Habitatge("2", "Carrer Girona 8", "Figueres", "17600", TipusImmoble.Casa, 140, 6, 3, "Joan Pujol", "12345678A"),   // família nombrosa + menors
//     new Habitatge("3", "Carrer Nou 1", "Figueres", "17600", TipusImmoble.Terreny, 500, 0, 0, "Joan Pujol", "88888888A"),   // terreny a Figueres
//     new Habitatge("4", "Carrer del Port 5", "Roses", "17480", TipusImmoble.Casa, 180, 7, 2, "Joan Pujol", "88888888A"),    // família nombrosa + menors a Roses (no ha de sortir a Figueres)

//     // Marta Vila (87654321B) - només a Figueres, sense família nombrosa ni menors (cas simple)
//     new Habitatge("5", "Avinguda Catalunya 3", "Figueres", "17600", TipusImmoble.Casa, 100, 2, 0, "Marta Vila", "87654321B"),
//     new Habitatge("6", "Carrer Ample 7", "Figueres", "17600", TipusImmoble.Pis, 75, 2, 0, "Marta Vila", "87654321B"),

//     // Pere Soler (34567890C) - família nombrosa però sense menors
//     new Habitatge("7", "Carrer Tramuntana 9", "Figueres", "17600", TipusImmoble.Casa, 200, 6, 0, "Pere Soler", "34567890C"),  // família nombrosa sense menors

//     // Anna Serra (45678901D) - menors però no família nombrosa
//     new Habitatge("8", "Passeig Maritim 40", "Figueres", "17600", TipusImmoble.Casa, 95, 3, 2, "Anna Serra", "45678901D"),   // menors però no família nombrosa

//     // Laura Bosch (56789012E) - tots els tipus d'immoble al mateix poble
//     new Habitatge("9", "Carrer Nou 3", "Roses", "17480", TipusImmoble.Pis, 60, 1, 0, "Laura Bosch", "56789012E"),
//     new Habitatge("10", "Carrer Nou 5", "L'Escala", "17130", TipusImmoble.Casa, 120, 4, 1, "Laura Bosch", "56789012E"),         // menors
//     new Habitatge("11", "Carrer Nou 7", "Roses", "17480", TipusImmoble.Terreny, 300, 0, 0, "Laura Bosch", "56789012E"),

//     // David Roca (67890123F) - habitant de Vallfort de les Manies (descompte 25% secret)
//     new Habitatge("12", "Carrer Major 1", "Vallfort de les Manies", "17970", TipusImmoble.Casa, 250, 7, 3, "David Roca", "67890123F"),  // família nombrosa + menors + descompte secret
//     new Habitatge("13", "Carrer Major 3", "Vallfort de les Manies", "17970", TipusImmoble.Terreny, 1000, 0, 0, "David Roca", "67890123F"),

//     // Núria Casas (78901234G) - habitant de Bellpeluda del Camí (descompte 25% secret)
//     new Habitatge("14", "Avinguda Pau 2", "L'Escala", "17130", TipusImmoble.Pis, 80, 5, 0, "Nuria Casas", "78901234G"),
//     new Habitatge("15", "Avinguda Pau 4", "Bellpeluda del Camí", "17971", TipusImmoble.Casa, 150, 4, 2, "Nuria Casas", "78901234G"),    // menors

//     // Ajuntament (P1706200I) - només terrenys
//     new Habitatge("16", "Zona Industrial", "Roses", "17480", TipusImmoble.Terreny, 2000, 0, 0, "Ajuntament de Roses", "P1706200I"),
//     new Habitatge("17", "Zona Nord", "Roses", "17480", TipusImmoble.Terreny, 1500, 0, 0, "Ajuntament de Roses", "P1706200I"),

// };

app.MapGet("/", () => "Funciona");
//llista habitatges
app.MapGet("/llistadhabitatges", () => habitatges);

//llistar habitatges per propietari

app.MapGet("/Habitatges/{Dni}", (string Dni) => {
List<Habitatge> habitatgeMateixPropietari = new();
for ( int i =0; i<habitatges.Count; i++)
    {
        if(habitatges[i].DNIContribuent == Dni)
        {
            habitatgeMateixPropietari.Add(habitatges[i]);
        }
    }

  return habitatgeMateixPropietari;
});

//buscar habitatge per ID

app.MapGet("/Habitatges/{Id}", (string Id) => {

for ( int i =0; i<habitatges.Count; i++)
    {
        if(habitatges[i].Identificador == Id)
        {
                return habitatges[i]; 
        }
    }
    return habitatges[0];
});

//Consultar la quantitat d’habitatges que hi ha en un poble (a partir del codi postal)
app.MapGet("/Poblacio/{CodiPostal}", (string CodiPostal) => {
List<Habitatge> totalHabitatgeMateixCodiPostal = new();
for ( int i =0; i<habitatges.Count; i++)
    {
        if(habitatges[i].CodiPostal == CodiPostal)
        {
            totalHabitatgeMateixCodiPostal.Add(habitatges[i]);
        }
    }

  return totalHabitatgeMateixCodiPostal.Count;
});
//Llistar els habitatges d’un poble 
app.MapGet("/PoblacioLlistaHabitatges/{CodiPostal}", (string CodiPostal) => {
List<Habitatge> habitatgesMateixPoble = new();
for ( int i =0; i<habitatges.Count; i++)
    {
        if(habitatges[i].CodiPostal == CodiPostal)
        {
            habitatgesMateixPoble.Add(habitatges[i]);
        }
    }

  return habitatgesMateixPoble;
});

//Llistar els contribuents d’un CP
app.MapGet("/ContribuentsMateixPoble/{CodiPostal}", (string CodiPostal) => {
Dictionary<string,string> ContribuentsMateixPoble = new();
for ( int i =0; i<habitatges.Count; i++)
    {
        if(habitatges[i].CodiPostal == CodiPostal)
        {
            ContribuentsMateixPoble[habitatges[i].DNIContribuent]=habitatges[i].NomContribuent;
        }
    }

  return ContribuentsMateixPoble;
});

//Llistar els contribuents d’un poble CP
app.MapGet("/ContribuentsMateixPoble2/{Poblacio}", (string Poblacio) => {
List<string> ContribuentsMateixPoble = new();
for ( int i =0; i<habitatges.Count; i++)
    {
        if(habitatges[i].Poblacio == Poblacio)
        {
            ContribuentsMateixPoble.Add(habitatges[i].NomContribuent);
        }
    }

  return ContribuentsMateixPoble;
});

//Llistar les cases on viuen més de 5 persones (perquè se’ls hi ha de fer descompte)
app.MapGet("/HabitatgesNumerosos", () => {
List<Habitatge> habitatgeFamiliaNumerosa = new();
for ( int i =0; i<habitatges.Count; i++)
    {
        if(habitatges[i].EsFamiliaNumerosa == true)
        {
            habitatgeFamiliaNumerosa.Add(habitatges[i]);
        }
    }

  return habitatgeFamiliaNumerosa;
});



app.Run();
