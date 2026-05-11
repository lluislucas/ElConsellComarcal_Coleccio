
using System.Text.Json.Nodes;
Dictionary<string, Dictionary<string, List<JsonNode>>> habitatgesXcp = new();

string Id;

JsonNode HabitatgeSeleccionat;

string Cp;
string Dni;

int Num_casesTot = 0;
int Num_pisosTot = 0;
int Num_terrenysTot = 0;
bool Descomptetassa = false;

int M2casesTot = 0;
int M2pisosTot = 0;
int M2terrenysTot = 0;
double QuotaGeneral = 0;



Console.WriteLine("Digues un CP");
Cp = Console.ReadLine();

////Console.WriteLine("Digues un DNI");
//Dni = Console.ReadLine();

using var http = new HttpClient();
var json1 = await http.GetStringAsync("http://localhost:5238/llistadhabitatges");


JsonArray llistaHabitatges = JsonNode.Parse(json1)?.AsArray();

for (int i = 0; i < llistaHabitatges.Count; i++)
{
    string cparevisar = llistaHabitatges[i]["codiPostal"].ToString();

    if (Cp == cparevisar || Cp == null)
    {

        string dniContribuentArevisar = llistaHabitatges[i]["dniContribuent"].ToString();

        if (!habitatgesXcp.ContainsKey(cparevisar))
        {
            habitatgesXcp[cparevisar] = new();
        }

        if (!habitatgesXcp[cparevisar].ContainsKey(dniContribuentArevisar))
        {
            habitatgesXcp[cparevisar][dniContribuentArevisar] = new();
        }
        habitatgesXcp[cparevisar][dniContribuentArevisar].Add(llistaHabitatges[i]);
    }

}



foreach(var kvp in habitatgesXcp[Cp])
{
    List<JsonNode> habitatgesContribuent = kvp.Value;
        

for (int i = 0; i < habitatgesContribuent.Count; i++)
{
   

    double quota_subtotal = 0;
    double quota_total = 0;
    int m2DeLaPropietat = int.Parse(habitatgesContribuent[i]["metresQuadrats"].ToString());

    if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "0")// tipusImmoble[0])
    {

        Num_casesTot++;
        M2casesTot += m2DeLaPropietat;
        quota_subtotal = 0.998 * m2DeLaPropietat;
        double descompteFamiliaNumerosa = 0;
        double incrementMenors = 0;

        if (int.Parse(habitatgesContribuent[i]["habitantsImmoble"].ToString()) >= 5)
        {
            descompteFamiliaNumerosa = quota_subtotal * 0.1;
        }
        if (int.Parse(habitatgesContribuent[i]["numMenorsImmoble"].ToString()) > 0)
        {
            incrementMenors = quota_subtotal * 0.05;
        }

        quota_total = quota_subtotal - descompteFamiliaNumerosa + incrementMenors;

    }

    if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "1")
    {
        Num_pisosTot++;
        M2pisosTot += m2DeLaPropietat;
        quota_subtotal = 0.996 * m2DeLaPropietat;
        double descompteFamiliaNumerosa = 0;
        double incrementMenors = 0;

        if (int.Parse(habitatgesContribuent[i]["habitantsImmoble"].ToString()) >= 5)
        {
            descompteFamiliaNumerosa = quota_subtotal * 0.1;
        }
        if (int.Parse(habitatgesContribuent[i]["numMenorsImmoble"].ToString()) > 0)
        {
            incrementMenors = quota_subtotal * 0.05;
        }

        quota_total = quota_subtotal - descompteFamiliaNumerosa + incrementMenors;

    }

    if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "2")
    {
        Num_terrenysTot++;
        M2terrenysTot += m2DeLaPropietat;
        quota_subtotal = 0.996 * m2DeLaPropietat;
    }
    if (habitatgesContribuent[i]["codiPostal"].ToString() == "17970" || habitatgesContribuent[i]["codiPostal"].ToString() == "17971")
    {
        quota_total = quota_total - quota_subtotal * 0.25;
    }
    QuotaGeneral += quota_total;
}

Console.WriteLine("--------------------------------------------------");
Console.WriteLine("Consell Comarcal - Taxa Especial sobre Habitatges");
Console.WriteLine($"");

Console.WriteLine($"Nom:{habitatgesXcp[Cp][kvp.Key][0]["nomContribuent"]}");
Console.WriteLine($"Poblacio:{habitatgesXcp[Cp][kvp.Key][0]["poblacio"]}");


if (M2casesTot > 0)
{
    Console.WriteLine($"Cases:{Num_casesTot} - {M2casesTot}m2");
}
else
{
    Console.WriteLine($"Cases:0");
}

if (M2pisosTot > 0)
{
    Console.WriteLine($"Pisos:{Num_pisosTot} - {M2pisosTot}m2");
}
else
{
    Console.WriteLine($"Pisos:0");
}

if (M2terrenysTot > 0)
{
    Console.WriteLine($"Terrenys:{Num_terrenysTot} - {M2terrenysTot} m2");
}
else
{
    Console.WriteLine($"Terrenys:0");
}

Console.WriteLine($"");

Console.WriteLine($"QUOTA:{Math.Round(QuotaGeneral, 2)}€ ");
Console.WriteLine("--------------------------------------------------");
}