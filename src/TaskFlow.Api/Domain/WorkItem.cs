namespace TaskFlow.Api.Domain;

public enum WorkItemStatus
{
    Todo,
    Doing,
    Done
}

public class WorkItem
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Title { get; set; }
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Todo;
    public string? Assignee { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAt { get; set; }
}
