using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using backend.Helpers;

namespace backend.Entities;

public class Exam
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public ExamType Type { get; set; }
    [ForeignKey("CreatedById")] 
    public int CreatedById { get; set; }
    
    [JsonIgnore]
    public User? CreatedBy { get; set; }
}
