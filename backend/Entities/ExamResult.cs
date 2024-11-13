namespace backend.Entities;

public class ExamResult
{
    public int Id { get; set; }
    public int CorrectAnswers { get; set; }
    public Exam Exam { get; set; }
    public User Student { get; set; }
}