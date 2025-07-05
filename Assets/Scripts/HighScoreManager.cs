using UnityEngine;

public class HighScoreManager : SingletonBase<HighScoreManager>
{
    private const string HIGH_SCORE_KEY = "HighScore";

    public int HighScore { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        LoadHighScore();
    }

    public bool TrySetNewHighScore(int currentScore)
    {
        if (currentScore > HighScore)
        {
            HighScore = currentScore;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, HighScore);
            PlayerPrefs.Save();
            return true; // ✅ New high score!
        }
        return false; // ✅ No new high score.
    }

    private void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }
}
