namespace LibraryManagement.Domain.Abstracts;

public abstract class Entity(Guid? id = null)
{
    public Guid Id { get; private init; } = id ?? Guid.NewGuid();
}
