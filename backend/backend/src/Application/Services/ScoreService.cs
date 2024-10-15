namespace backend.Application.Services;

public class ScoreService
{
    public int CalculeScore(int correctAnswers, TimeSpan totalTime)
    {
        int scoreBase = correctAnswers * 10;
        int timeBonus = totalTime.TotalSeconds <= 180 ? 5 : 0;

        return scoreBase + timeBonus;
    }
}