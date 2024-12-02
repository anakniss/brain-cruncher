using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Entities;

public class ExamResult
{
    public int Id { get; set; }
    public int CorrectAnswers { get; set; }
    [ForeignKey("ExamId")]
    public int ExamId { get; set; }
    [JsonIgnore]
    public Exam? Exam { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    [JsonIgnore]
    public User? Student { get; set; }
    public DateTime CreatedAt { get; set; }
}