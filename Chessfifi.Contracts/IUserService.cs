using Chessfifi.Contracts.Dto;

namespace Chessfifi.Services.Service;

public interface IUserService
{
    User GetUser(string id);
}
