using System.Collections;
using UnityEngine;

public class GiraffeMovement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Collider2D collider2d;

    [Header("Keys")]
    [SerializeField] KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] KeyCode moveRightKey = KeyCode.D;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Modifiers")]
    [SerializeField, Range(1, 500)] float horizontalAcceleration = 150f;
    [SerializeField, Range(1, 500)] float horizontalMaxSpeed = 15f;
    [SerializeField] float jumpForce = 4.5f;
    [SerializeField, Range(0, 1f)] float slide = 0.5f;
    [SerializeField, Range(0, 1f)] float movementSmoothing = 0.01f;
    [SerializeField] float collisionBoxLength = 0.5f;

    Rigidbody2D rb;
    KeyCode currentInput = KeyCode.None;
    Vector2 currentVelocity = Vector2.zero;
    // Need this flag as FixedUpdate() might not pick up on quick tap of the jump key.
    bool wasJumpPressed;
    private bool isJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (groundLayer.value == 0)
        {
            Debug.LogWarning(
                $"{nameof(GiraffeMovement)}: It seems that the layer mask has not been set. Jumping might not work!");
        }
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleHorizontalMovement();
    }

    private void HandleHorizontalMovement()
    {
        var velocity = rb.velocity;
        var switchedDirection = (currentInput == moveLeftKey && velocity.x > 0f) ||
                                (currentInput == moveRightKey && velocity.x < 0f);

        var result = switchedDirection ? 0f : velocity.x;
        var rotation = transform.rotation.y;

        if (currentInput == moveLeftKey && result > -horizontalMaxSpeed)
        {
            result -= horizontalAcceleration * Time.deltaTime;
            rotation = 180f;
        }
        else if (currentInput == moveLeftKey)
        {
            result = -horizontalMaxSpeed;
            rotation = 180f;
        }
        else if (currentInput == moveRightKey && result < horizontalMaxSpeed)
        {
            result += horizontalAcceleration * Time.deltaTime;
            rotation = 0;
        }
        else if (currentInput == moveRightKey)
        {
            result = horizontalMaxSpeed;
            rotation = 0;
        }
        else
        {
            result *= slide;
        }

        RotateGiraffe(rotation);

        rb.velocity = Vector2.SmoothDamp(
            current: velocity,
            target: new Vector2(result, velocity.y),
            currentVelocity: ref currentVelocity,
            smoothTime: movementSmoothing);
    }

    private void RotateGiraffe(float rotation)
    {
        transform.rotation = new Quaternion(0, rotation, 0, 0);
    }

    private void HandleJump()
    {
        if (!wasJumpPressed) return;

        wasJumpPressed = false;

        if (IsColliding(Vector2.down) && !isJumping)
        {
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        isJumping = true;
        rb.AddForce(new Vector2(0f, jumpForce * rb.gravityScale * rb.mass), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    bool IsColliding(Vector2 direction)
    {
        var playerBounds = collider2d.bounds;
        var hit = Physics2D.BoxCast(
            origin: playerBounds.center,
            size: playerBounds.size,
            angle: 0f,
            direction: direction,
            distance: collisionBoxLength,
            layerMask: groundLayer);

        return hit.collider != null;
    }

    private void GetInput()
    {
        wasJumpPressed = Input.GetKey(jumpKey);

        if (Input.GetKey(moveRightKey))
        {
            currentInput = moveRightKey;
        }
        else if (Input.GetKey(moveLeftKey))
        {
            currentInput = moveLeftKey;
        }
        else
        {
            currentInput = KeyCode.None;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var otherRb = collision.collider.GetComponent<Rigidbody2D>();

        if (otherRb == null)
        {
            return;
        }

        if (otherRb.CompareTag("Head"))
        {
            otherRb.velocity = Vector2.zero;
            otherRb.angularVelocity = 0f;
        }
    }
}
