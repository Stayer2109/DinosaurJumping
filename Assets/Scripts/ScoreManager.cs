using System;
using UnityEngine;

public class ScoreManager : SingletonBase<ScoreManager>
{
    [SerializeField] private readonly float scoreRate = 10f; // Score per second
    [SerializeField] private bool isScoring = true;
    private float score = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (isScoring && GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            score += scoreRate * Time.deltaTime;
        }
    }

    public void AddScore(float amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return Mathf.FloorToInt(score);
    }

    public void AddBonus(int amount)
    {
        score += amount;
    }

    public void StopScoring()
    {
        isScoring = false;
    }

    public void ResumeScoring()
    {
        isScoring = true;
    }

    public void ResetScore()
    {
        score = 0f;
    }
}
