namespace Optsol.EventDriven.Components.Core.Domain.Entities;

public interface IDomainEvent
{
    public Guid ModelId { get; }
    public int ModelVersion { get; }
    public DateTime When { get; }
}