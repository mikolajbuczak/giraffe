using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] KeyCode moveRightKey = KeyCode.D;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Setup")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private float movePower = 5; // The force added to the ball to move it.
    [SerializeField] private bool useTorque = true; // Whether or not to use torque to move the ball.
    [SerializeField] private float maxVelocity = 25; // The maximum velocity the ball can rotate at.
    [SerializeField] private float jumpPower = 2; // The force added to the ball when it jumps.

    bool wasJumpPressed;
    Vector2 currentDirection = Vector3.zero; 
    private const float groundRayLength = 1f; // The length of the ray to check if the ball is grounded.
    private Rigidbody2D rb;
    private CircleCollider2D collider2d;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleMovement(currentDirection);
        HandleJump();
    }

    private void GetInput()
    {
        wasJumpPressed = Input.GetKey(jumpKey);

        if (Input.GetKey(moveRightKey))
        {
            currentDirection = Vector3.right;
        }
        else if (Input.GetKey(moveLeftKey))
        {
            currentDirection = Vector3.left;
        }
        else
        {
            currentDirection = Vector3.zero;
        }
    }

    private void HandleMovement(Vector3 moveDirection)
    {
        // If using torque to rotate the ball...
        if (useTorque)
        {
            // ... add torque around the axis defined by the move direction.
            rb.AddTorque(-moveDirection.x * movePower);
        }
        else
        {
            // Otherwise add force in the move direction.
            rb.AddForce(moveDirection * movePower);
        }

        ClampAngularVelocity();
    }

    private void HandleJump()
    {
        if (!wasJumpPressed) return;

        wasJumpPressed = false;

        if (IsColliding(Vector2.down))
        {
            rb.AddForce(new Vector2(0f, jumpPower * rb.gravityScale), ForceMode2D.Impulse);
        }
    }

    private void ClampAngularVelocity()
    {
        if (rb.angularVelocity < -maxVelocity) { rb.angularVelocity = -maxVelocity; }
        if (rb.angularVelocity > maxVelocity) { rb.angularVelocity = maxVelocity; }
    }

    private bool IsColliding(Vector2 direction)
    {
        var playerBounds = collider2d.bounds;
        var hit = Physics2D.BoxCast(
            origin: playerBounds.center,
            size: playerBounds.size,
            angle: 0f,
            direction: direction,
            distance: 0.5f,
            layerMask: groundLayer);

        return hit.collider != null;
    }
}