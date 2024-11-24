using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace backend.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    [ForeignKey("ExamId")]
    public Exam Exam { get; set; }
    public DateTime CreatedAt { get; set; }
}