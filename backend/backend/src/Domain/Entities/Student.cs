namespace backend.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? StudentId { get; set; }
    public string? Password { get; set; }
    private int TotalScore { get; set; }
    public DateTime CreatedAt { get; set; }

    public Student(string name, string email, string studentId, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        StudentId = studentId;
        Password = password;
        TotalScore = 0;
        CreatedAt = DateTime.Now;
    }

    public void UpdateScore(int score)
    {
        TotalScore += score;
    }
}