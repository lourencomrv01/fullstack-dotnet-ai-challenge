using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TaskFlow.Api.Tests;

/// <summary>
/// Contrato executavel do endpoint PATCH /api/tasks/{id}/status.
/// Estes testes sao a traducao 1:1 das regras que devem estar na sua spec.
/// Nao altere as asserts: ajuste a implementacao.
/// </summary>
public class StatusTransitionTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public StatusTransitionTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<JsonElement> CreateTaskAsync(string title, string? assignee = null)
    {
        var response = await _client.PostAsJsonAsync("/api/tasks", new { title, assignee });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>(Json);
    }

    private Task<HttpResponseMessage> PatchStatusAsync(string id, string status) =>
        _client.PatchAsJsonAsync($"/api/tasks/{id}/status", new { status });

    [Fact(DisplayName = "R1 - Todo -> Doing e uma transicao valida e retorna 200")]
    public async Task Todo_To_Doing_Is_Allowed()
    {
        var task = await CreateTaskAsync("R1", "ana");

        var response = await PatchStatusAsync(task.GetProperty("id").GetString()!, "Doing");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(Json);
        Assert.Equal("Doing", body.GetProperty("status").GetString());
    }

    [Fact(DisplayName = "R2 - Todo -> Done e proibido (nao se pula etapa) e retorna 409")]
    public async Task Todo_To_Done_Is_Rejected()
    {
        var task = await CreateTaskAsync("R2", "ana");

        var response = await PatchStatusAsync(task.GetProperty("id").GetString()!, "Done");

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact(DisplayName = "R3 - Concluir sem responsavel retorna 422")]
    public async Task Cannot_Complete_Without_Assignee()
    {
        var task = await CreateTaskAsync("R3");
        var id = task.GetProperty("id").GetString()!;
        await PatchStatusAsync(id, "Doing");

        var response = await PatchStatusAsync(id, "Done");

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact(DisplayName = "R4 - Concluir preenche completedAt; reabrir limpa completedAt")]
    public async Task Completing_Sets_CompletedAt_And_Reopening_Clears_It()
    {
        var task = await CreateTaskAsync("R4", "bruno");
        var id = task.GetProperty("id").GetString()!;
        await PatchStatusAsync(id, "Doing");

        var completed = await PatchStatusAsync(id, "Done");
        Assert.Equal(HttpStatusCode.OK, completed.StatusCode);
        var completedBody = await completed.Content.ReadFromJsonAsync<JsonElement>(Json);
        Assert.NotEqual(JsonValueKind.Null, completedBody.GetProperty("completedAt").ValueKind);

        var reopened = await PatchStatusAsync(id, "Doing");
        Assert.Equal(HttpStatusCode.OK, reopened.StatusCode);
        var reopenedBody = await reopened.Content.ReadFromJsonAsync<JsonElement>(Json);
        Assert.Equal(JsonValueKind.Null, reopenedBody.GetProperty("completedAt").ValueKind);
    }

    [Fact(DisplayName = "R5 - Status desconhecido retorna 400")]
    public async Task Unknown_Status_Is_Rejected()
    {
        var task = await CreateTaskAsync("R5", "ana");

        var response = await PatchStatusAsync(task.GetProperty("id").GetString()!, "Arquivado");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "R6 - Id inexistente retorna 404")]
    public async Task Unknown_Task_Returns_NotFound()
    {
        var response = await PatchStatusAsync(Guid.NewGuid().ToString(), "Doing");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
