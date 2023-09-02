using Chessfifi.Domain.UserAgg;
using Chessfifi.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Chessfifi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public UserRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IdentityUser GetUser(string id)
    {
        var player = _unitOfWork.Context.Users
            .First(x => x.Id == id);
        return player;
    }


}