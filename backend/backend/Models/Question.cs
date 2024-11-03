namespace backend.Models;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<Alternative> Options { get; set; }
}