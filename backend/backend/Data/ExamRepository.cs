using backend.Models;

namespace backend.Data;

public class ExamRepository
{
    private readonly QuizDbContext _context;

    public ExamRepository(QuizDbContext context)
    {
        _context = context;
        
    }
    
    public async Task AddAsync(Exam exam) => await _context.Exams.AddAsync(exam);
}