using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserExamController : ControllerBase
{
    private readonly DataContext _context;

    public UserExamController(DataContext context)
    {
        _context = context;
    }
  
    [HttpPost]
    public async Task<IActionResult> CreateUserExam([FromBody] UserExam userExam)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userExam.UserId);
        if (!userExists)
        {
            return BadRequest("User with the provided UserId does not exist.");
        }
        
        var examExists = await _context.Exams.AnyAsync(e => e.Id == userExam.ExamId);
        if (!examExists)
        {
            return BadRequest("Exam with the provided ExamId does not exist.");
        }
        
        var userExamExists = await _context.UserExams
            .AnyAsync(ue => ue.UserId == userExam.UserId && ue.ExamId == userExam.ExamId);

        if (userExamExists)
        {
            return Conflict(new { Message = "UserExam already exists with the provided UserId and ExamId." });
        }
        
        var newUserExam = new UserExam
        {
            UserId = userExam.UserId,
            ExamId = userExam.ExamId,
            CreatedAt = userExam.CreatedAt
        };

        _context.UserExams.Add(newUserExam);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "UserExam created successfully.",
            UserExam = new
            {
                newUserExam.UserId,
                newUserExam.ExamId,
                newUserExam.CreatedAt
            }
        });
    }

    [HttpGet("GetAllUserExams")]
    public async Task<ActionResult<IEnumerable<UserExam>>> GetAllUserExams()
    {
        var userExams = await _context.UserExams.ToListAsync();
        return Ok(userExams);
    }
    
    [HttpGet("{userId}/{examId}")]
    public async Task<IActionResult> GetUserExam(int userId, int examId)
    {
        var userExam = await _context.UserExams
            .Where(ue => ue.UserId == userId && ue.ExamId == examId)
            .Select(ue => new
            {
                ue.UserId,
                ue.ExamId,
                ue.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (userExam == null)
        {
            return NotFound("UserExam not found.");
        }

        return Ok(userExam);
    }

    
    [HttpPut("{userId}/{examId}")]
    public async Task<IActionResult> UpdateUserExam(int userId, int examId, UserExam userExam)
    {
        if (userId != userExam.UserId || examId != userExam.ExamId)
        {
            return BadRequest("UserId or ExamId mismatch.");
        }

        var existingUserExam = await _context.UserExams
            .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.ExamId == examId);

        if (existingUserExam == null)
        {
            return NotFound("UserExam not found.");
        }
        
        existingUserExam.CreatedAt = userExam.CreatedAt;

        await _context.SaveChangesAsync();

        return Ok(new { Message = "UserExam updated.", UserExam = existingUserExam });
    }
    
    [HttpDelete("{userId}/{examId}")]
    public async Task<IActionResult> DeleteUserExam(int userId, int examId)
    {
        var userExam = await _context.UserExams
            .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.ExamId == examId);

        if (userExam == null)
        {
            return NotFound("UserExam not found.");
        }

        _context.UserExams.Remove(userExam);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "UserExam deleted." });
    }
}