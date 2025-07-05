using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public int targetFPS = 60;

    void Awake()
    {
        Application.targetFrameRate = targetFPS;
        QualitySettings.vSyncCount = 0;

        DontDestroyOnLoad(gameObject); // stays across scenes if you want
    }
}
