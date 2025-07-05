using UnityEngine;

public class CoinListener : MonoBehaviour
{
    void OnEnable()
    {
        Coin.OnCoinCollected += HandleCoinCollected;
    }

    void OnDisable()
    {
        Coin.OnCoinCollected -= HandleCoinCollected;
    }

    private void HandleCoinCollected(int value, Vector3 pos)
    {
        // Increase score
        ScoreManager.Instance.AddBonus(value);

        // Play sound
        AudioManager.Instance.PlaySFX("coin");

        // Optional: trigger VFX
        VFXManager.Instance.SpawnEffect("CoinSparkle", pos);
    }
}
