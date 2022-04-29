namespace Optsol.EventDriven.Components.Core.Domain;

public abstract class Aggregate : IAggregate 
{
    private readonly Queue<IEvent> _pendingEvents = new();
    private readonly Queue<IFailureEvent> _failureEvents = new();
    protected int Version { get; set; } = 0;
    protected int NextVersion
    {
        get => Version + 1;
    }
    public Guid Id { get; protected set; }
    public IEnumerable<IEvent> PendingEvents
    {
        get => _pendingEvents.AsEnumerable();
    }

    public IEnumerable<IFailureEvent> FailureEvents
    {
        get => _failureEvents.AsEnumerable();
    }
    
    protected Aggregate() {}
    public Aggregate(IEnumerable<IEvent> persistedEvents)
    {
        if (persistedEvents.Any())
        {
            ApplyPersistedEvents(persistedEvents);
        }
    }
    
    protected void RaiseEvent<TEvent>(TEvent pendingEvent) where TEvent : IEvent
    {
        _pendingEvents.Enqueue(pendingEvent);
        Apply(pendingEvent);
        Version = pendingEvent.ModelVersion;
    }

    protected abstract void Apply(IEvent @event);
    
    private void ApplyPersistedEvents(IEnumerable<IEvent> events)
    {
        foreach (var e in events)
        {
            Apply(e);
            Version = e.ModelVersion;
        }
    }
    
    public void Commit()
    {
        _pendingEvents.Clear();
        _failureEvents.Clear();
    }
}