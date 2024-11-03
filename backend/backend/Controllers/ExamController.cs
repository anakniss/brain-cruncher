using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/exams")]
public class ExamController : ControllerBase
{
    private readonly ExamService _examService;

    public ExamController(ExamService examService)
    {
        _examService = examService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateExam([FromBody] Exam exam)
    {
        await _examService.CreateExamAsync(exam);
        return Ok();
    }
}