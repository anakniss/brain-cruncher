using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExamResultController : ControllerBase
{
    private readonly DataContext _context;

    public ExamResultController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet("GetAllExamResults")]
    public async Task<ActionResult<List<ExamResult>>> GetAllExamResults()
    {
        var examResults = await _context.ExamResults.ToListAsync();
        
        if (!examResults.Any())
            return NotFound("No Exam Result found.");
        
        var result = examResults.Select(e => new
        {
            e.Id,
            e.CorrectAnswers,
            e.CreatedAt,
            e.ExamId,
            e.UserId
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExamResult(int id)
    {
        var examResult = await _context.ExamResults
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.CorrectAnswers,
                e.CreatedAt,
                e.ExamId,
                e.UserId
            })
            .FirstOrDefaultAsync();

        if (examResult == null)
        {
            return NotFound("Exam Result not found.");
        }

        return Ok(examResult);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAlternative(ExamResult examResult)
    {
        var examExists = await _context.Exams.AnyAsync(e => e.Id == examResult.ExamId);
        if (!examExists)
        {
            return BadRequest("Exam with the provided ExamId does not exist.");
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == examResult.UserId);
        if (!userExists)
        {
            return BadRequest("User with the provided UserId does not exist.");
        }
        
        examResult.CreatedAt = DateTime.UtcNow;
        _context.ExamResults.Add(examResult);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExamResult), new { id = examResult.Id }, examResult);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExamResult(int id, [FromBody] ExamResult examResult)
    {
        if (id != examResult.Id)
        {
            return BadRequest("ExamResult ID mismatch.");
        }

        var existingExamResult = await _context.ExamResults.FindAsync(id);
        if (existingExamResult == null)
        {
            return NotFound("ExamResult not found.");
        }
        
        var examExists = await _context.Exams.AnyAsync(e => e.Id == examResult.ExamId);
        if (!examExists)
        {
            return BadRequest("Exam with the provided ExamId does not exist.");
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == examResult.UserId);
        if (!userExists)
        {
            return BadRequest("User with the provided UserId does not exist.");
        }
        
        existingExamResult.CorrectAnswers = examResult.CorrectAnswers;
        existingExamResult.ExamId = examResult.ExamId;
        existingExamResult.UserId = examResult.UserId;
        existingExamResult.CreatedAt = examResult.CreatedAt;

        await _context.SaveChangesAsync();

        return Ok(new { Message = "ExamResult updated successfully.", ExamResult = existingExamResult });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExamResult(int id)
    {
        var examResult = await _context.ExamResults.FindAsync(id);
        if (examResult == null)
        {
            return NotFound("ExamResult not found.");
        }

        _context.ExamResults.Remove(examResult);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "ExamResult deleted successfully." });
    }

    
    [HttpGet("GetExamResultByUser/{userId}")]
    public async Task<IActionResult> GetExamResultByUser(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return BadRequest("Invalid UserId.");
        }

        var examResults = await _context.ExamResults
            .Where(e => e.UserId == userId)
            .Select(e => new
            {
                e.Id,
                e.CorrectAnswers,
                e.CreatedAt,
                e.ExamId,
                e.UserId
            })
            .ToListAsync();

        if (!examResults.Any())
        {
            return NotFound("No Exam Result found for the provided UserId.");
        }

        return Ok(examResults);
    }

    [HttpGet("GetExamCountByUserId/{userId}")]
    public async Task<ActionResult<int>> GetExamCountByUserId(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return BadRequest("Invalid user id.");

        var examCount = await _context.ExamResults
            .Where(e => e.UserId == userId)
            .CountAsync();
        return Ok(examCount);
    }

}