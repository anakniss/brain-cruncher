using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExamController : ControllerBase
{
    private readonly DataContext _context;

    public ExamController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet("GetAllExams")]
    public async Task<ActionResult<List<Exam>>> GetAllExams()
    {
        var exams = await _context.Exams.ToListAsync();
        
        if (!exams.Any())
            return NotFound("No exams found.");
        
        var result = exams.Select(e => new
        {
            e.Id,
            e.Title,
            e.Type,
            e.CreatedAt,
            e.CreatedById
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExam(int id)
    {
        var exam = await _context.Exams
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Type,
                e.CreatedAt,
                e.CreatedById
            })
            .FirstOrDefaultAsync();

        if (exam == null)
        {
            return NotFound("Exam not found.");
        }

        return Ok(exam);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateExam(Exam exam)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == exam.CreatedById);
        if (!userExists)
            return BadRequest("Invalid CreatedById.");
        
        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();

        return Ok(exam);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateExam(Exam exam)
    {
        var dbExam = await _context.Exams.FindAsync(exam.Id);
        if (dbExam is null)
            return NotFound("Exam not found.");
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == exam.CreatedById);
        if (!userExists)
            return BadRequest("Invalid CreatedById.");
        
        dbExam.Title = exam.Title;
        dbExam.Type = exam.Type;
        dbExam.CreatedAt = exam.CreatedAt;
        dbExam.CreatedById = exam.CreatedById;
        
        await _context.SaveChangesAsync();
        
        return Ok(new
        {
            Message = "Exam updated successfully.",
            Exam = new
            {
                dbExam.Id,
                dbExam.Title,
                dbExam.Type,
                dbExam.CreatedAt,
                dbExam.CreatedById
            }
        });
    }

    [HttpDelete]
    public async Task<ActionResult<List<Exam>>> DeleteExam(int id)
    {
        var dbExam = await _context.Exams.FindAsync(id);
        if (dbExam is null)
            return NotFound("Exam not found.");

        _context.Exams.Remove(dbExam);
        await _context.SaveChangesAsync();
    
        return Ok(new
        {
            Message = "Exam deleted successfully.",
            Id = id
        });
    }


    [HttpGet("GetExamsByUserId/{userId}")]
    public async Task<ActionResult<List<Exam>>> GetExamsByUserId(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return BadRequest("Invalid user id.");

        var exams = await _context.Exams
            .Where(e => e.CreatedById == userId)
            .ToListAsync();

        if (!exams.Any())
            return NotFound("No exams found.");

        return Ok(exams);
    }
}