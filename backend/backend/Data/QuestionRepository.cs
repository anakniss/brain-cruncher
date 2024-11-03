using backend.Models;

namespace backend.Data;

public class QuestionRepository
{
    private readonly QuizDbContext _context;

    public QuestionRepository(QuizDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Question question) => await _context.Questions.AddAsync(question);
}