using Microsoft.AspNetCore.Identity;

namespace Chessfifi.Domain.UserAgg;

public interface IUserRepository
{
    IdentityUser GetUser(string id);
}
