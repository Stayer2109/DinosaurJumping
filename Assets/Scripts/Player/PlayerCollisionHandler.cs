using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;  // Reference to player Animator
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tree"))
        {
            OnPlayerDeath();
        }
    }

    public void OnPlayerDeath()
    {
        animator.SetBool("isJumping", false);
        animator.SetTrigger("Die");
        GameManager.Instance.GameOver();
    }
}
