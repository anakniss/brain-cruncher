using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class ExamResult
{
    public int Id { get; set; }
    public int CorrectAnswers { get; set; }
    public int ExamId { get; set; }
    [ForeignKey("ExamId")]
    public Exam Exam { get; set; } = null!;
    
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User Student { get; set; } = null!;
}