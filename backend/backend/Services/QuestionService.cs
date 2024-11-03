using backend.Data;
using backend.Models;

namespace backend.Services;

public class QuestionService
{
    private readonly QuestionRepository _questionRepository;

    public QuestionService(QuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task AddQuestionAsync(Question question) => await _questionRepository.AddAsync(question);
}