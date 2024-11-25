using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly DataContext _context;

    public QuestionController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetAllQuestions()
    {
        var questions = await _context.Questions.ToListAsync();

        if (!questions.Any())
            return NotFound("No questions found");

        var result = questions.Select(e => new
        {
            e.Id,
            e.Text,
            e.CreatedAt,
            e.ExamId
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> GetQuestion(int id)
    {
        var question = await _context.Questions
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Text,
                e.CreatedAt,
                e.ExamId
            })
            .FirstOrDefaultAsync();

        if (question == null)
        {
            return NotFound("Question not found.");
        }

        return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<Question>> CreateQuestion([FromBody] Question question)
    {
        var questionExists = await _context.Exams.AnyAsync(u => u.Id == question.ExamId);
        if (!questionExists)
            return BadRequest("Invalid ExamId");

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return Ok(question);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestion(int id, [FromBody] Question question)
    {
        var dbQuestion = await _context.Questions.FindAsync(question.Id);
        if (dbQuestion is null)
            return NotFound("Exam not found.");
        
        var examExists = await _context.Exams.AnyAsync(u => u.Id == question.ExamId);
        if (!examExists)
            return BadRequest("Invalid ExamId.");
        
        dbQuestion.Text = question.Text;
        dbQuestion.CreatedAt = question.CreatedAt;
        dbQuestion.ExamId = question.ExamId;
        
        await _context.SaveChangesAsync();
        
        return Ok(new
        {
            Message = "Question updated successfully.",
            Exam = new
            {
                dbQuestion.Id,
                dbQuestion.Text,
                dbQuestion.CreatedAt,
                dbQuestion.ExamId,
            }
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var dbQuestion = await _context.Questions.FindAsync(id);
        if (dbQuestion is null)
            return NotFound("Question not found.");

        _context.Questions.Remove(dbQuestion);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Question deleted successfully.",
            Id = id
        });
    }
}