using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{

    public enum GameState
    {
        Start,
        Playing,
        Paused,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentState = GameState.Start;
    }

    void Start()
    {
        AudioManager.Instance.PlayMainMenuMusic();
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
            ScoreManager.Instance.ResumeScoring();
        }
        Time.timeScale = 1f;
        AudioManager.Instance.StopMainMenuMusic();
        AudioManager.Instance.PlayMusic();
    }

    public void PauseGame()
    {
        SetState(GameState.Paused);
        ScoreManager.Instance.StopScoring();
        Time.timeScale = 0f;
        AudioManager.Instance.FadeOutMusic(0.5f);
        GlobalUIManager.Instance.MainGameUI.ShowPausePanel();
    }

    public void ResumeGame()
    {
        SetState(GameState.Playing);
        ScoreManager.Instance.ResumeScoring();
        Time.timeScale = 1f;
        AudioManager.Instance.PlayMusic();
        GlobalUIManager.Instance.MainGameUI.HidePausePanel();
    }

    public void GameOver()
    {
        SetState(GameState.GameOver);
        ScoreManager.Instance.StopScoring();
        Time.timeScale = 0f;
        AudioManager.Instance.FadeOutMusic(0.5f);
        AudioManager.Instance.PlaySFX("game-over");
        bool didBeatHighScore = HighScoreManager.Instance.TrySetNewHighScore(ScoreManager.Instance.GetScore());
        GlobalUIManager.Instance.MainGameUI.ShowGameOverPanel(didBeatHighScore);
    }

    public void ReturnToMainMenu()
    {
        SetState(GameState.Start);
        Time.timeScale = 1f; // Reset time scale
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMainMenuMusic();
    }

    public void RestartGame()
    {
        // Reset the game state
        SetState(GameState.Playing);
        Time.timeScale = 1f; // Reset time scale
        AudioManager.Instance.PlayMusicFromBeginning(); // Ensure music restarts from the beginning

        // Reset score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
            ScoreManager.Instance.ResumeScoring();
        }
    }

    public void QuitGame()
    {
        Application.Quit();

        // NOTE: This won't quit in the Editor. Use this for testing:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetState(GameState gameState)
    {
        CurrentState = gameState;
    }
}
