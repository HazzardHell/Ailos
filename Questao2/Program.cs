using Newtonsoft.Json;
using System.Net.Http;
using System;

public class Program
{
    private static readonly HttpClient client = new HttpClient();
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals("Paris Saint-Germain", 2013);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals("Chelsea", 2014);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        string url = $"https://jsonmock.hackerrank.com/api/football_matches?team1={team}&year={year}";

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Lança uma exceção em caso de erro na requisição

            string json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            if (data != null && data.data != null)
            {
                int totalGoals = 0;
                foreach (var match in data.data)
                {
                    if (match.team1 == team && match.team1goals != null)
                        totalGoals += Convert.ToInt32(match.team1goals);
                    if (match.team2 == team && match.team2goals != null)
                        totalGoals += Convert.ToInt32(match.team2goals);
                }
                return totalGoals;
            }
            else
            {
                Console.WriteLine("Erro: Dados inválidos retornados pela API.");
                return 0;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro ao fazer a requisição HTTP: {ex.Message}");
            return 0;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao desserializar JSON: {ex.Message}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
            return 0;
        }
    }


}