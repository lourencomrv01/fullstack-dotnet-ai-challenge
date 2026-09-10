using System.Text.Json.Serialization;
using TaskFlow.Api.Domain;
using TaskFlow.Api.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryStore>();
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

var store = app.Services.GetRequiredService<InMemoryStore>();
store.Add(new WorkItem { Title = "Configurar ambiente", Status = WorkItemStatus.Done, Assignee = "ana", CompletedAt = DateTimeOffset.UtcNow });
store.Add(new WorkItem { Title = "Escrever a spec da feature", Status = WorkItemStatus.Doing, Assignee = "bruno" });
store.Add(new WorkItem { Title = "Revisar o backlog", Status = WorkItemStatus.Todo });

var api = app.MapGroup("/api/tasks");

api.MapGet("/", (InMemoryStore db) => Results.Ok(db.All()));

api.MapGet("/{id:guid}", (Guid id, InMemoryStore db) =>
    db.Find(id) is { } item ? Results.Ok(item) : Results.NotFound());

api.MapPost("/", (CreateTaskRequest request, InMemoryStore db) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["title"] = ["Title e obrigatorio."]
        });
    }

    var item = db.Add(new WorkItem { Title = request.Title.Trim(), Assignee = request.Assignee });
    return Results.Created($"/api/tasks/{item.Id}", item);
});

// ---------------------------------------------------------------------------
// A feature que voce vai implementar sera entregue no inicio da sessao:
// um arquivo de testes (o contrato executavel) e um stub de spec em specs/.
//
// A ordem de trabalho esperada e sempre a mesma:
//   1. preencher a spec,  2. conduzir a IA a partir dela,  3. verificar com dotnet test.
//
// Ate la, nao ha nada a fazer aqui alem de garantir que o ambiente roda.
// ---------------------------------------------------------------------------

app.Run();

public record CreateTaskRequest(string Title, string? Assignee);
public record UpdateStatusRequest(string Status);

public partial class Program;
