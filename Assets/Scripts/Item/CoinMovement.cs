using System.Collections;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    void OnEnable()
    {
        StartCoroutine(DespawnAfterTime());
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        transform.Translate(Vector3.left * DifficultyManager.Instance.CurrentSpeed * Time.deltaTime);
    }

    IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
