namespace backend.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    private int TotalScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserRole Role { get; set; }

    public User(string name, string email, string password, UserRole role)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Password = password;
        TotalScore = 0;
        CreatedAt = DateTime.Now;
        Role = role;
    }

    public void UpdateScore(int score)
    {
        TotalScore += score;
    }
}

public enum UserRole
{
    Student,
    Teacher
}
