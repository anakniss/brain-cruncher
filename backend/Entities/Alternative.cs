using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class Alternative
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCorrect { get; set; }
    [ForeignKey("QuestionId")]
    public Question Question { get; set; }
}