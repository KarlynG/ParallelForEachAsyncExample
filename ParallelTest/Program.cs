using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;

var pokemonHandler = new[]
{
    "pikachu",
    "charizard",
    "mankey",
    "arceus",
    "slowpoke",
    "abra",
    "tepig",
    "glaceon"
};

using HttpClient client = new()
{
    BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon/"),
};
client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DotNet", "6"));

ParallelOptions parallelOptions = new()
{
    MaxDegreeOfParallelism = -1
};


async Task NormalForeach()
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    foreach (var pokemon in pokemonHandler)
    {
        var poke = await client.GetFromJsonAsync<Pokemon>("https://pokeapi.co/api/v2/pokemon/" + pokemon);
        Console.WriteLine($"Name: {poke.Name}\nWeight: {poke.Weight}\n");
    }
    stopwatch.Stop();
    Console.WriteLine("");
    Console.WriteLine("Completed Task in: " + stopwatch.Elapsed.TotalSeconds);
}

async Task ParallelForeach()
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    await Parallel.ForEachAsync(pokemonHandler, parallelOptions, async (uri, token) =>
    {
        var poke = await client.GetFromJsonAsync<Pokemon>(uri, token);

        Console.WriteLine($"Name: {poke.Name}\nBio: {poke.Weight}\n");
    });
    stopwatch.Stop();
    Console.WriteLine("");
    Console.WriteLine("Completed Task in: " + stopwatch.Elapsed.TotalSeconds);
}

Console.WriteLine($"\n Parallel Foreach async\n ");
await ParallelForeach();

public class Pokemon
{
    public string Name { get; set; }
    public int Weight { get; set; }
}