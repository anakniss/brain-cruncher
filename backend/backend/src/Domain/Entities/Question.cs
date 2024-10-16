namespace backend.Domain.Entities;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string[] Options { get; set; }
    public int CorrectIndex { get; set; }
    public DifficultLevel Difficult { get; set; }
    public int MaxResponseTimeSeconds { get; set; }

    public Question(string text, string[] options, int correctIndex, DifficultLevel difficult)
    {
        if (options.Length != 3)
        {
            throw new ArgumentException("A pergunta deve ter exatamente 3 alternativas");
        }
        
        Id = Guid.NewGuid();
        Text = text;
        Options = options;
        CorrectIndex = correctIndex;
        Difficult = difficult;
        MaxResponseTimeSeconds = 180; //180 segundos
    }
}

public enum DifficultLevel
{
    Beginner,
    Intermediate,
    Advanced
}