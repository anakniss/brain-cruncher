using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    [ForeignKey("ExamId")]
    public int ExamId { get; set; }
    public Exam Exam { get; set; }
    public DateTime CreatedAt { get; set; }
}