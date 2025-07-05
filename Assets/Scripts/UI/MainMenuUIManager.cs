using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;

    void Start()
    {
        highScoreText.text = "Hi: " + HighScoreManager.Instance.HighScore.ToString();
    }

    public void OnPlayButtonClicked()
    {
        GameManager.Instance.StartGame();
        SceneManager.LoadScene("MainGame"); // Replace with your actual game scene name
    }

    public void OnQuitButtonClicked()
    {
        GameManager.Instance.QuitGame();
    }
}
