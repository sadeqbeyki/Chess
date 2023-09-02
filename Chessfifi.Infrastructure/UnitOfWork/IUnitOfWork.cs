namespace Chessfifi.Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ApplicationDbContext Context { get; }
    void Commit();
}
