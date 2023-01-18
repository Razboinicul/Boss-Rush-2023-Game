using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpStrength = 10;
    [SerializeField] private float gravityScale = 20.0f;

    // Components attached to the player
    [Header("Components attached to the player")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    // Other
    [Header("Other")]
    private bool isGrounded = false;
    public float maxHealth = 100;
    [SerializeField] private float currentHealth = 0;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = gravityScale;
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        UpdateIsGrounded();
        HandleHorizontalMovement();
        HandleJumping();
    }

    private void UpdateIsGrounded()
    {
        Bounds colliderBounds = capsuleCollider.bounds;
        float colliderRadius = capsuleCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        // Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        this.isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != capsuleCollider)
                {
                    this.isGrounded = true;
                    break;
                }
            }
        }
    }

    private void HandleHorizontalMovement()
    {
        Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();
        rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, rigidBody.velocity.y);
    }

    private void HandleJumping()
    {
        bool jumpPressed = InputManager.GetInstance().GetJumpPressed();
        if (isGrounded && jumpPressed)
        {
            isGrounded = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpStrength);
        }
    }
}
