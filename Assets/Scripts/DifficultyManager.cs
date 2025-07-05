using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 7f;
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float acceleration = 0.01f; // How fast it speeds up

    [SerializeField] private GameObject treePrefab_Level2;
    private bool treeLevel2Unlocked = false;

    [SerializeField] private GameObject treePrefab_Level3;
    private bool treeLevel3Unlocked = false;

    public float CurrentSpeed { get; private set; }

    private float survivalTime = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        CurrentSpeed = baseSpeed;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        UpdateSurvivalTime();
        UpdateGameSpeed();
        CheckScoreBasedUnlocks();
    }

    private void UpdateSurvivalTime()
    {
        survivalTime += Time.deltaTime;
    }

    private void UpdateGameSpeed()
    {
        float targetSpeed = Mathf.Clamp(baseSpeed + (survivalTime * acceleration), baseSpeed, maxSpeed);
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, targetSpeed, Time.deltaTime * 2f);
    }

    private void CheckScoreBasedUnlocks()
    {
        int score = ScoreManager.Instance.GetScore();

        // Unlock tree level 2
        if (!treeLevel2Unlocked && score >= 300)
        {
            EnumBasedPoolManager.Instance.AddPrefabToPool(ObstacleType.Tree, treePrefab_Level2);
            treeLevel2Unlocked = true;
        }

        // Unlock tree level 3
        if (!treeLevel3Unlocked && score >= 600)
        {
            EnumBasedPoolManager.Instance.AddPrefabToPool(ObstacleType.Tree, treePrefab_Level3);
            treeLevel3Unlocked = true;
        }
    }

}
