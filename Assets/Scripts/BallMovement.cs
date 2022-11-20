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
    [SerializeField] float collisionBoxLength = 0.5f;

    bool wasJumpPressed;
    Vector2 currentDirection = Vector3.zero;
    private Rigidbody2D rb;
    private bool isJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleMovement(currentDirection);
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

        if (IsColliding() && !isJumping)
        {
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        isJumping = true;
        rb.AddForce(new Vector2(0f, jumpPower * rb.gravityScale), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private void ClampAngularVelocity()
    {
        if (rb.angularVelocity < -maxVelocity) { rb.angularVelocity = -maxVelocity; }
        if (rb.angularVelocity > maxVelocity) { rb.angularVelocity = maxVelocity; }
    }

    private bool IsColliding()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, collisionBoxLength, groundLayer);
        Debug.Log(hit.collider.gameObject.name);

        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var otherRb = collision.collider.GetComponent<Rigidbody2D>();

        if (otherRb == null)
        {
            return;
        }

        if (otherRb.CompareTag("Body") || otherRb.CompareTag("Head"))
        {
            otherRb.velocity = Vector2.zero;
            otherRb.angularVelocity = 0f;
        }
    }
}