using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("comarcal");

List<Habitatge> habitant = [];


app.MapGet("/habitant",() =>
{
    
    List<Habitatge> habitatges = [];

    using (MySqlConnection connexio = new(connectionString))
    {
        connexio.Open();
        MySqlCommand comanda = new(
@"SELECT e.id as edifici_id,direccio,p.nom as poble_nom,p.codi_postal,c.id as contribuent_id,c.nom as contribuent_nom,c.dni as contribuent_dni,t.id as tipus_id,t.nom as tipus_nom,habitants,menors,metres_quadrats 
from edificacions e
left join pobles p on p.id = e.poble_id 
left join tipus t on t.id = e.tipus_id 
left join contribuents c on c.id = e.contribuent_id
order by e.id;",connexio
);
        using (var reader = comanda.ExecuteReader())
        {
            while(reader.Read())
            {
                var h = new Habitatge
                {
                    Identificador = reader.GetInt32("edifici_id").ToString(),
                    Adreça = reader.GetString("direccio"),
                    Població = reader.GetString("poble_nom"),
                    CodiPostal = reader.GetInt32("codi_postal"),
                    TipusImmoble = reader.GetString("tipus_nom"),
                    MetresQuadrats = reader.GetInt32("metres_quadrats"),
                    QuantitatPersonesQueViuenMajors = reader.GetInt32("habitants"),
                    QuantitatPersonesQueViuenMenors = reader.GetInt32("menors"),
                    NomPagaTaxa = reader.GetString("contribuent_nom"),
                    DNIPagaTaxa = reader.GetString("contribuent_dni"),
                };
                habitatges.Add(h);
                
            }
            return habitatges;
        }


    }
});

app.MapGet("/", () => "Benvingut a la API del Consell Comarcal!");

app.MapGet("/habitant/{Identificador}",(string Identificador) =>
{
    using(MySqlConnection connexio = new (connectionString))
    {
        connexio.Open();
        
        MySqlCommand comanda = new (
@"select e.id as edifici_id,direccio,p.nom as poble_nom,p.codi_postal,c.id as contribuent_id,c.nom as contribuent_nom,c.dni as contribuent_dni,t.id as tipus_id,t.nom as tipus_nom,habitants,menors,metres_quadrats 
from edificacions e
left join pobles p on p.id = e.poble_id 
left join tipus t on t.id = e.tipus_id 
left join contribuents c on c.id = e.contribuent_id
where e.id = @id
order by e.id;",connexio
        );

        comanda.Parameters.AddWithValue("@id", Identificador);

        using (var reader = comanda.ExecuteReader())
        {
            if(reader.Read())
            {
                 return Results.Ok(new Habitatge
                {
                    Identificador = reader.GetInt32("edifici_id").ToString(),
                    Adreça = reader.GetString("direccio"),
                    Població = reader.GetString("poble_nom"),
                    CodiPostal = reader.GetInt32("codi_postal"),
                    TipusImmoble = reader.GetString("tipus_nom"),
                    MetresQuadrats = reader.GetInt32("metres_quadrats"),
                    QuantitatPersonesQueViuenMajors = reader.GetInt32("habitants"),
                    QuantitatPersonesQueViuenMenors = reader.GetInt32("menors"),
                    NomPagaTaxa = reader.GetString("contribuent_nom"),
                    DNIPagaTaxa = reader.GetString("contribuent_dni"),
                });
                
            }
        }
    }
    return Results.NotFound();
});


app.MapGet("/habitatges/{DNIPagaTaxa}", (string DNIPagaTaxa) =>
{
    List<Habitatge> habitatgesPropietari = new();
    using(MySqlConnection connexio = new (connectionString))
    {
        connexio.Open();
        
        MySqlCommand comanda = new (
@"select e.id as edifici_id,direccio,p.nom as poble_nom,p.codi_postal,c.id as contribuent_id,c.nom as contribuent_nom,c.dni as contribuent_dni,t.id as tipus_id,t.nom as tipus_nom,habitants,menors,metres_quadrats 
from edificacions e
left join pobles p on p.id = e.poble_id 
left join tipus t on t.id = e.tipus_id 
left join contribuents c on c.id = e.contribuent_id
where c.dni = @dni
order by c.dni;",connexio
        );

        comanda.Parameters.AddWithValue("@dni", DNIPagaTaxa);

        using (var reader = comanda.ExecuteReader())
        {
            while(reader.Read())
            {
                 habitatgesPropietari.Add(new Habitatge
                    {
                    Identificador = reader.GetInt32("edifici_id").ToString(),
                    Adreça = reader.GetString("direccio"),
                    Població = reader.GetString("poble_nom"),
                    CodiPostal = reader.GetInt32("codi_postal"),
                    TipusImmoble = reader.GetString("tipus_nom"),
                    MetresQuadrats = reader.GetInt32("metres_quadrats"),
                    QuantitatPersonesQueViuenMajors = reader.GetInt32("habitants"),
                    QuantitatPersonesQueViuenMenors = reader.GetInt32("menors"),
                    NomPagaTaxa = reader.GetString("contribuent_nom"),
                    DNIPagaTaxa = reader.GetString("contribuent_dni"),
                });
                
            }
        }
    }
    if(habitatgesPropietari.Count == 0) return Results.NotFound("No s'ha trobat habitatges amb aquest DNI");
    return Results.Ok(habitatgesPropietari);

});

app.MapGet("/QuantitatHabitatges/{CodiPostal}",(int CodiPostal) =>
{
    using(MySqlConnection connexio = new (connectionString))
    {
        connexio.Open();
        
        MySqlCommand comanda = new (
@"select count(*) from edificacions e 
join pobles p ON e.poble_id = p.id
where p.codi_postal = @cp",connexio
        );

        comanda.Parameters.AddWithValue("@cp", CodiPostal);

       var total = comanda.ExecuteScalar();
    
    return Results.Ok(total);

    }
});

app.MapGet("/habitants/{Població}",(string Població) =>
{
    List<Habitatge> llista = new();
 using(MySqlConnection connexio = new (connectionString))
    {
        connexio.Open();
        
        MySqlCommand comanda = new (
@"select e.id as edifici_id,direccio,p.nom as poble_nom,p.codi_postal,c.id as contribuent_id,c.nom as contribuent_nom,c.dni as contribuent_dni,t.id as tipus_id,t.nom as tipus_nom,habitants,menors,metres_quadrats 
from edificacions e
left join pobles p on p.id = e.poble_id 
left join tipus t on t.id = e.tipus_id 
left join contribuents c on c.id = e.contribuent_id
where p.nom = @poble
order by c.dni;",connexio
        );

        comanda.Parameters.AddWithValue("@poble", Població);

        using (var reader = comanda.ExecuteReader())
        {
            while(reader.Read())
            {
                 llista.Add(new Habitatge
                {
                    Identificador = reader.GetInt32("edifici_id").ToString(),
                    Adreça = reader.GetString("direccio"),
                    Població = reader.GetString("poble_nom"),
                    CodiPostal = reader.GetInt32("codi_postal"),
                    TipusImmoble = reader.GetString("tipus_nom"),
                    MetresQuadrats = reader.GetInt32("metres_quadrats"),
                    QuantitatPersonesQueViuenMajors = reader.GetInt32("habitants"),
                    QuantitatPersonesQueViuenMenors = reader.GetInt32("menors"),
                    NomPagaTaxa = reader.GetString("contribuent_nom"),
                    DNIPagaTaxa = reader.GetString("contribuent_dni"),
                });
                
            }
        }
    }
    if(llista.Count == 0) return Results.NotFound("No s'ha trobat habitatges en aquest poble");

    return Results.Ok(llista);

});

List<Habitatge> LlistaPoble = new List<Habitatge>();

app.MapGet("/llistaPoble/{Població}",(string Població) =>
{
    foreach(var pob in habitant)
    {
        if(pob.Població == Població) LlistaPoble.Add(pob);
    }
    
    if(LlistaPoble.Count == 0)
    {
        return Results.NotFound();
    }
    return Results.Ok(LlistaPoble);

});

app.MapGet("/contribuent/{Població}",(string Població) => 
{
    List<DadesPersones> llista = new();
 using(MySqlConnection connexio = new (connectionString))
    {
        connexio.Open();
        
        MySqlCommand comanda = new (
@"select e.id as edifici_id,direccio,p.nom as poble_nom,p.codi_postal,c.id as contribuent_id,c.nom as contribuent_nom,c.dni as contribuent_dni,t.id as tipus_id,t.nom as tipus_nom,habitants,menors,metres_quadrats 
from edificacions e
left join pobles p on p.id = e.poble_id 
left join tipus t on t.id = e.tipus_id 
left join contribuents c on c.id = e.contribuent_id
where p.nom = @poble
order by c.dni;",connexio
        );

        comanda.Parameters.AddWithValue("@poble", Població);

        using (var reader = comanda.ExecuteReader())
        {
            while(reader.Read())
            {
                 llista.Add(new DadesPersones
                {

                    Nom = reader.GetString("contribuent_nom"),
                    Dni = reader.GetString("contribuent_dni"),
                });
                
            }
        }
    }
    if(llista.Count == 0) return Results.NotFound("No s'ha trobat habitatges en aquest poble");

    return Results.Ok(llista);

});


List<Habitatge> cases = new List<Habitatge>();

app.MapGet("/casa/{Població}",(string Població) =>
{
    using(MySqlConnection connexio = new (connectionString))
    {
        List<Habitatge> llistaCasesGran = new();
        connexio.Open();
        
        MySqlCommand comanda = new (
@"select e.id as edifici_id,direccio,p.nom as poble_nom,p.codi_postal,c.id as contribuent_id,c.nom as contribuent_nom,c.dni as contribuent_dni,t.id as tipus_id,t.nom as tipus_nom,habitants,menors,metres_quadrats 
from edificacions e
left join pobles p on p.id = e.poble_id 
left join tipus t on t.id = e.tipus_id 
left join contribuents c on c.id = e.contribuent_id
where p.nom = @poble AND (habitants + menors)> 5
order by e.id;",connexio
        );

        comanda.Parameters.AddWithValue("@poble", Població);

        using (var reader = comanda.ExecuteReader())
        {
            while(reader.Read())
            {
                llistaCasesGran.Add(new Habitatge
                {
                    Identificador = reader.GetInt32("edifici_id").ToString(),
                    Adreça = reader.GetString("direccio"),
                    Població = reader.GetString("poble_nom"),
                    CodiPostal = reader.GetInt32("codi_postal"),
                    TipusImmoble = reader.GetString("tipus_nom"),
                    MetresQuadrats = reader.GetInt32("metres_quadrats"),
                    QuantitatPersonesQueViuenMajors = reader.GetInt32("habitants"),
                    QuantitatPersonesQueViuenMenors = reader.GetInt32("menors"),
                    NomPagaTaxa = reader.GetString("contribuent_nom"),
                    DNIPagaTaxa = reader.GetString("contribuent_dni"),
                });
                
            }
        }
    }
    return Results.NotFound();
});

app.MapGet("/poble",() =>
{
    List<string> pobles = new();
 using(MySqlConnection connexio = new (connectionString))
    {
        connexio.Open();
        
        MySqlCommand comanda = new (
@"select distinct p.nom as poble_nom
from pobles p
order by p.nom;",connexio
        );


        using (var reader = comanda.ExecuteReader())
        {
            while(reader.Read())
            {
                string NomPoble = reader.GetString("poble_nom");
                pobles.Add(NomPoble);
            }
        }
    }
    if(pobles.Count == 0) return Results.NotFound("No s'ha trobat habitatges en aquest poble");

    return Results.Ok(pobles);
});



app.Run();

public class Habitatge
{
    public string Identificador {get;set;}
    public string Adreça {get;set;}
    public string Població{get;set;}
    public int CodiPostal {get;set;}
    public string TipusImmoble {get;set;}
    public int MetresQuadrats{get;set;}
    public int QuantitatPersonesQueViuenMajors {get;set;}
    public int QuantitatPersonesQueViuenMenors {get;set;}
    public string NomPagaTaxa {get;set;}
    public string DNIPagaTaxa {get;set;}

}

public class DadesPersones
{
    public string Nom {get;set;}
    public string Dni {get;set;}
}