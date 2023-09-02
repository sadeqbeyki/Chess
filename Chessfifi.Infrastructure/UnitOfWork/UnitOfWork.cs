namespace Chessfifi.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public ApplicationDbContext Context { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        Context = context;
    }

    //todo -> Commit When Http Request Completed
    public void Commit() => Context.SaveChanges();

    public void Dispose() => Context.Dispose();
}
