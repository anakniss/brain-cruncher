using backend.Data;
using backend.Models;

namespace backend.Services;

public class ExamService
{
    private readonly ExamRepository _examRepository;

    public ExamService(ExamRepository examRepository)
    {
        _examRepository = examRepository;
    }

    public async Task CreateExamAsync(Exam exam) => await _examRepository.AddAsync(exam);
}