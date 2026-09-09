using System.Collections.Concurrent;
using TaskFlow.Api.Domain;

namespace TaskFlow.Api.Storage;

public class InMemoryStore
{
    private readonly ConcurrentDictionary<Guid, WorkItem> _items = new();

    public IReadOnlyCollection<WorkItem> All() =>
        _items.Values.OrderBy(i => i.CreatedAt).ToList();

    public WorkItem? Find(Guid id) => _items.TryGetValue(id, out var item) ? item : null;

    public WorkItem Add(WorkItem item)
    {
        _items[item.Id] = item;
        return item;
    }
}
