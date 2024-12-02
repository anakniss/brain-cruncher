using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Entities;

public class Alternative
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCorrect { get; set; }
    [ForeignKey("QuestionId")]
    public int QuestionId { get; set; }
    
    [JsonIgnore]
    public Question? Question { get; set; } = null!;
}