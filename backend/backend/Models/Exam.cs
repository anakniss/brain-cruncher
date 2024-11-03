using backend.Helpers;

namespace backend.Models;

public class Exam
{
    public int Id { get; set; }
    public ExamType Type { get; set; }
    public List<Question> Questions { get; set; }
    public User CreatedBy { get; set; }
    public List<User> EnrolledStudents { get; set; }
}