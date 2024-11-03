using backend.Data;
using backend.Models;

namespace backend.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user != null && user.Password == password)
            return user;

        return null;
    }
}