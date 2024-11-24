using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<Alternative> Alternatives { get; set; }
    [ForeignKey("ExamId")]
    public Exam Exam { get; set; }
}