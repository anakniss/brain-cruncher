using backend.Models;

namespace backend.Data;

public class UserRepository
{
    private readonly QuizDbContext _context;

    public UserRepository(QuizDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);
    public async Task<User> GetByEmailAsync(string email) => await _context.Users.FindAsync(email);
    public async Task AddAsync(User user) => await _context.Users.AddAsync(user);
}