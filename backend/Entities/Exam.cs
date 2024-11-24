using backend.Helpers;

namespace backend.Entities;

public class Exam
{
    public int Id { get; set; }
    public ExamType Type { get; set; }
    public List<Question> Questions { get; set; }
    public User CreatedBy { get; set; }
}
