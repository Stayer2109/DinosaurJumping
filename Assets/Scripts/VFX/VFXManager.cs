using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    [SerializeField] private GameObject coinVFXPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnEffect(string effectName, Vector3 position)
    {
        if (effectName == "CoinSparkle" && coinVFXPrefab != null)
        {
            GameObject fx = Instantiate(coinVFXPrefab, position, Quaternion.identity);
            Destroy(fx, 1f); // Auto-destroy after 1 sec
        }
    }
}
