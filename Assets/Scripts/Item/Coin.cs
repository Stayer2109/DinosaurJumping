using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 100;
    public static event Action<int, Vector3> OnCoinCollected;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCoinCollected?.Invoke(value, transform.position);
            Destroy(gameObject);
        }
    }
}
