using Chessfifi.Domain.UserAgg;
using Chessfifi.Services.Dto;
using Microsoft.AspNetCore.Identity;

namespace Chessfifi.Services.Service;

public interface IUserService
{
    User GetUser(string id);
}

public class UserService : IUserService
{
    private IUserRepository _userRepository;

    public UserService(IUserRepository playerRepo)
    {
        _userRepository = playerRepo;
    }

    public User GetUser(string id)
    {
        var dbUser = _userRepository.GetUser(id);
        return FillUserDto(dbUser);
    }

    private User FillUserDto(IdentityUser dbUser)
    {
        var userDto = new User();
        userDto.Id = dbUser.Id;
        userDto.IsEmailConfirmed = dbUser.EmailConfirmed;
        return userDto;
    }
}
