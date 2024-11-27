using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    [ForeignKey("ExamId")]
    public int ExamId { get; set; }
    
    [JsonIgnore]
    public Exam? Exam { get; set; }
    public DateTime CreatedAt { get; set; }
}