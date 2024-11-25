using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class ExamResult
{
    public int Id { get; set; }
    public int CorrectAnswers { get; set; }
    [ForeignKey("ExamId")]
    public int ExamId { get; set; }
    public Exam Exam { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public User Student { get; set; }
    public DateTime CreatedAt { get; set; }
}