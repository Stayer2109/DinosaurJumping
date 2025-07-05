using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    private Rigidbody2D rb;
    private Vector2 vecGravity;
    private bool isJumpHeld;

    // Ground check variables
    [Header("Ground Check")]
    private bool isGrounded;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Jump variables
    [Header("Jump Settings")]
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private float fallMultiplier = 2f;
    [SerializeField] private float jumpCutMultiplier = 2f;

    // Animator
    [SerializeField] private Animator animator;

    void OnEnable()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.PlayerMovement.Enable();
        playerInputAction.PlayerMovement.Jump.performed += OnJump;
        playerInputAction.PlayerMovement.Jump.canceled += OnJumpReleased;
    }

    void OnDisable()
    {
        playerInputAction.PlayerMovement.Jump.performed -= OnJump;
        playerInputAction.PlayerMovement.Jump.canceled -= OnJumpReleased;
        playerInputAction.PlayerMovement.Disable();
    }

    void Start()
    {
        vecGravity = -Physics2D.gravity;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity -= fallMultiplier * Time.deltaTime * vecGravity;
        }
        else if (rb.linearVelocity.y > 0f && !isJumpHeld)
        {
            rb.linearVelocity -= jumpCutMultiplier * Time.deltaTime * vecGravity;
        }

        // 🔥 Update jump animation based on grounded state
        animator.SetBool("isJumping", !isGrounded);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            animator.SetBool("isJumping", true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumpHeld = true;
            AudioManager.Instance.PlaySFX("jump");
        }
    }

    private void OnJumpReleased(InputAction.CallbackContext ctx)
    {
        isJumpHeld = false;
    }
}
