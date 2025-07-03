namespace LibraryManagement.Domain.Abstracts;

public abstract class Entity(Guid id)
{
    public Guid Id { get; private init; } = id;
}
