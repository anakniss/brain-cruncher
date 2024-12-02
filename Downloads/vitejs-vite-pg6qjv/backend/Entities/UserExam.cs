using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class UserExam
{
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    [ForeignKey("ExamId")]
    public int ExamId { get; set; }
    public User User { get; set; } = null!;
    public Exam Exam { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}