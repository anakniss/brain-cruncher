using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlternativeController : ControllerBase
{
    private readonly DataContext _context;

    public AlternativeController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet("GetAllAlternatives")]
    public async Task<ActionResult<List<Alternative>>> GetAllAlternatives()
    {
        var alternatives = await _context.Alternatives.ToListAsync();
        
        if (!alternatives.Any())
            return NotFound("No alternatives found.");
        
        var result = alternatives.Select(e => new
        {
            e.Id,
            e.Description,
            e.IsCorrect,
            e.QuestionId
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlternative(int id)
    {
        var alternative = await _context.Alternatives
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Description,
                e.IsCorrect,
                e.QuestionId
            })
            .FirstOrDefaultAsync();

        if (alternative == null)
        {
            return NotFound("Alternative not found.");
        }

        return Ok(alternative);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAlternative(Alternative alternative)
    {
        var questionExists = await _context.Questions.AnyAsync(u => u.Id == alternative.QuestionId);
        if (!questionExists)
            return BadRequest("Invalid QuestionId.");
        
        _context.Alternatives.Add(alternative);
        await _context.SaveChangesAsync();

        return Ok(alternative);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateAlternative(Alternative alternative)
    {
        var dbAlternative = await _context.Alternatives.FindAsync(alternative.Id);
        if (dbAlternative is null)
            return NotFound("Alternative not found.");
        
        var questionExists = await _context.Questions.AnyAsync(u => u.Id == alternative.QuestionId);
        if (!questionExists)
            return BadRequest("Invalid QuestionId.");
        
        dbAlternative.Id = alternative.Id;
        dbAlternative.Description = alternative.Description;
        dbAlternative.IsCorrect = alternative.IsCorrect;
        dbAlternative.QuestionId = alternative.QuestionId;
        
        await _context.SaveChangesAsync();
        
        return Ok(new
        {
            Message = "Alternative updated successfully.",
            Alternative = new
            {
                dbAlternative.Id,
                dbAlternative.Description,
                dbAlternative.IsCorrect,
                dbAlternative.QuestionId
            }
        });
    }

    [HttpDelete]
    public async Task<ActionResult<List<Exam>>> DeleteAlternative(int id)
    {
        var dbAlternative = await _context.Alternatives.FindAsync(id);
        if (dbAlternative is null)
            return NotFound("Alternative not found.");

        _context.Alternatives.Remove(dbAlternative);
        await _context.SaveChangesAsync();
    
        return Ok(new
        {
            Message = "Alternative deleted successfully.",
            Id = id
        });
    }
}