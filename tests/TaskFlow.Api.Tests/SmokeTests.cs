using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TaskFlow.Api.Tests;

/// <summary>
/// Testes de fumaca do scaffold. Servem para voce confirmar que o ambiente
/// esta funcionando antes da sessao. Devem passar sem que voce mude nada.
/// </summary>
public class SmokeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public SmokeTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact(DisplayName = "A API sobe e lista as tarefas de exemplo")]
    public async Task Lists_Seeded_Tasks()
    {
        var response = await _client.GetAsync("/api/tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var tasks = await response.Content.ReadFromJsonAsync<JsonElement>(Json);
        Assert.True(tasks.GetArrayLength() >= 3);
    }

    [Fact(DisplayName = "Criar tarefa devolve 201 e o status vem como string")]
    public async Task Creates_Task()
    {
        var response = await _client.PostAsJsonAsync("/api/tasks", new { title = "smoke", assignee = "ana" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<JsonElement>(Json);
        Assert.Equal("Todo", created.GetProperty("status").GetString());
    }

    [Fact(DisplayName = "Criar tarefa sem titulo devolve 400")]
    public async Task Rejects_Empty_Title()
    {
        var response = await _client.PostAsJsonAsync("/api/tasks", new { title = "   " });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
