using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalUIManager : SingletonBase<GlobalUIManager>
{
    private MainGameUIManager mainGameSceneUI;
    public MainGameUIManager MainGameUI => mainGameSceneUI;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainGame")
        {
            mainGameSceneUI = FindFirstObjectByType<MainGameUIManager>();
        }
        else
        {
            mainGameSceneUI = null;
        }
    }
}
