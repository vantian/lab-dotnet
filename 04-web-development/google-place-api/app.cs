// program.cs
// Single-file .NET 10 console app demonstrating REST calls to jsonplaceholder.typicode.com
// Run: dotnet run

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

using var http = new HttpClient { BaseAddress = new Uri("https://jsonplaceholder.typicode.com/") };

try
{
    Console.WriteLine("GET /posts (first 5)");
    var posts = await http.GetFromJsonAsync("posts", AppJsonContext.Default.PostArray, cts.Token);
    Print(posts?[..Math.Min(5, posts.Length)], AppJsonContext.Default.PostArray);

    Console.WriteLine("\nGET /posts/1");
    var post1 = await http.GetFromJsonAsync("posts/1", AppJsonContext.Default.Post, cts.Token);
    Print(post1, AppJsonContext.Default.Post);

    Console.WriteLine("\nPOST /posts (create)");
    var newPost = new Post { UserId = 10, Title = "Hello from .NET 10", Body = "Sample body" };
    var postResponse = await http.PostAsJsonAsync("posts", newPost, AppJsonContext.Default.Post, cts.Token);
    var created = await postResponse.Content.ReadFromJsonAsync(AppJsonContext.Default.Post, cts.Token);
    Console.WriteLine($"Status: {postResponse.StatusCode}");
    Print(created, AppJsonContext.Default.Post);

    Console.WriteLine("\nPUT /posts/1 (update)");
    var update = new Post { UserId = 1, Id = 1, Title = "Updated Title", Body = "Updated body" };
    var putResponse = await http.PutAsJsonAsync("posts/1", update, AppJsonContext.Default.Post, cts.Token);
    var updated = await putResponse.Content.ReadFromJsonAsync(AppJsonContext.Default.Post, cts.Token);
    Console.WriteLine($"Status: {putResponse.StatusCode}");
    Print(updated, AppJsonContext.Default.Post);

    Console.WriteLine("\nDELETE /posts/1");
    var delResponse = await http.DeleteAsync("posts/1", cts.Token);
    Console.WriteLine($"Status: {delResponse.StatusCode}");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation canceled.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

void Print<T>(T? obj, JsonTypeInfo<T> typeInfo) where T : class
{
    if (obj == null) { Console.WriteLine("(null)"); return; }
    Console.WriteLine(JsonSerializer.Serialize(obj, typeInfo));
}

public class Post
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
}

[JsonSerializable(typeof(Post))]
[JsonSerializable(typeof(Post[]))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
internal partial class AppJsonContext : JsonSerializerContext { }