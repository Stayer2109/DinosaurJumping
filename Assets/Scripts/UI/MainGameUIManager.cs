using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gamePanel;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI currentScoreTextPause;
    [SerializeField]
    private TextMeshProUGUI currentScoreTextGameOver;
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private TextMeshProUGUI quoteText;
    [SerializeField]
    private Material daydreamMaterialWithBlackOutline;
    [SerializeField]
    private Material daydreamMaterialWithWhiteOutline;

    private static readonly string[] QUOTE_NEW_HIGH_SCORE = new string[]
 {
    "New high score? Who are you, tryhard?",
    "Okay legend, calm down",
    "New record unlocked. Touch grass?",
    "High score flex, we see you.",
    "Dang, save some skill for the rest of us"
 };

    private static readonly string[] QUOTE_GAME_OVER = new string[]
    {
        "Game over. Skill issue?",
        "RIP. Try again, sweat",
        "Down bad. Next run maybe?",
        "L ratio. Run it back?",
        "At least you tried. Kinda (haha)"
    };

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
        }
    }

    #region Handle Button Click Events
    public void OnPauseButtonClicked()
    {
        GameManager.Instance.PauseGame();
    }

    public void OnResumeButtonClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnRestartButtonClicked()
    {
        // Reset the game
        GameManager.Instance.RestartGame();

        // Restart the current scene
        SceneManager.LoadScene("MainGame");
    }

    public void OnMainMenuButtonClicked()
    {
        GameManager.Instance.ReturnToMainMenu();
        SceneManager.LoadScene("_MainMenu");
    }

    public void OnQuitButtonClicked()
    {
        GameManager.Instance.QuitGame();
    }
    #endregion

    #region Pause Menu
    public void ShowPausePanel()
    {
        if (pausePanel == null)
        {
            Debug.LogError("Pause menu is not assigned in the UIManager.");
            return;
        }

        // Activate the panel first before animating
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);

        currentScoreTextPause.text = "Cur. Score: " + ScoreManager.Instance.GetScore().ToString();

        // Animate the pause menu to fade in (or scale, or move)
        pausePanel.GetComponent<CanvasGroup>().DOFade(1f, duration: 0.5f).SetUpdate(true);
    }

    public void HidePausePanel()
    {
        if (pausePanel == null)
        {
            Debug.LogError("Pause menu is not assigned in the UIManager.");
            return;
        }

        // Animate the pause menu to fade out (or scale, or move)
        pausePanel.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            pausePanel.SetActive(false); // Deactivate after animation
            gamePanel.SetActive(true);
        });
    }
    #endregion

    #region Game Over Panel
    public void ShowGameOverPanel(bool didBeatHighScore)
    {
        if (gameOverPanel == null)
        {
            Debug.LogError("Game Over panel is not assigned in the UIManager.");
            return;
        }

        // Activate the panel first before animating
        gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);

        Sequence gameOverSequence = DOTween.Sequence();

        currentScoreTextGameOver.text = "Cur. Score: " + ScoreManager.Instance.GetScore().ToString();
        highScoreText.text = "Hi. Score: " + HighScoreManager.Instance.HighScore.ToString();

        // Get a random quote
        quoteText.color = didBeatHighScore ? Color.green : Color.red;
        quoteText.color = didBeatHighScore ? Color.green : Color.red;
        quoteText.fontSharedMaterial = didBeatHighScore
            ? daydreamMaterialWithBlackOutline
            : daydreamMaterialWithWhiteOutline;
        quoteText.text = didBeatHighScore
        ? QUOTE_NEW_HIGH_SCORE[Random.Range(0, QUOTE_NEW_HIGH_SCORE.Length)]
        : QUOTE_GAME_OVER[Random.Range(0, QUOTE_GAME_OVER.Length)];

        // Animation sequence
        gameOverSequence.Append(gameOverPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.5f));
        gameOverSequence.Join(gameOverPanel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));

        gameOverSequence.SetUpdate(true);
    }

    public void HideGameOverPanel()
    {
        if (gameOverPanel == null)
        {
            Debug.LogError("Game Over panel is not assigned in the UIManager.");
            return;
        }

        Sequence gameOverSequence = DOTween.Sequence();

        gameOverSequence.Append(gameOverPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.5f));  // Fade out over 0.5 seconds
        gameOverSequence.Join(gameOverPanel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack));

        gameOverSequence.OnComplete(() =>
        {
            gameOverPanel.SetActive(false); // Deactivate after animation
            gamePanel.SetActive(true);
        });
    }
    #endregion
}
