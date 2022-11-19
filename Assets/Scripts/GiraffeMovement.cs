using UnityEngine;

public class GiraffeMovement : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] LayerMask groundLayer;

    [Header("Keys")]
    [SerializeField] KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] KeyCode moveRightKey = KeyCode.D;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Modifiers")]
    [SerializeField, Range(1, 500)] float horizontalAcceleration = 150f;
    [SerializeField, Range(1, 500)] float horizontalMaxSpeed = 15f;
    [SerializeField, Range(1, 100)] float jumpForce = 4.5f;
    [SerializeField, Range(0, 1f)] float slide = 0.5f;
    [SerializeField, Range(0, 1f)] float movementSmoothing = 0.01f;
    [SerializeField] float collisionBoxLength = 0.1f;
    [SerializeField, Range(0.01f, 1f)] float collisionBoxSize = 0.90f;

    Rigidbody2D rb;
    Collider2D collider2d;
    KeyCode currentInput = KeyCode.None;
    Vector2 currentVelocity = Vector2.zero;
    // Need this flag as FixedUpdate() might not pick up on quick tap of the jump key.
    bool wasJumpPressed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();

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

        if (currentInput == moveLeftKey && result > -horizontalMaxSpeed)
        {
            result -= horizontalAcceleration * Time.deltaTime;
        }
        else if (currentInput == moveLeftKey)
        {
            result = -horizontalMaxSpeed;
        }
        else if (currentInput == moveRightKey && result < horizontalMaxSpeed)
        {
            result += horizontalAcceleration * Time.deltaTime;
        }
        else if (currentInput == moveRightKey)
        {
            result = horizontalMaxSpeed;
        }
        else
        {
            result *= slide;
        }

        rb.velocity = Vector2.SmoothDamp(
            current: velocity,
            target: new Vector2(result, velocity.y),
            currentVelocity: ref currentVelocity,
            smoothTime: movementSmoothing);
    }

    private void HandleJump()
    {
        if (!wasJumpPressed) return;

        wasJumpPressed = false;

        if (IsColliding(Vector2.down))
        {
            Debug.Log("Jump");
            rb.AddForce(new Vector2(0f, jumpForce * rb.gravityScale * rb.mass), ForceMode2D.Impulse);
        }
    }

    bool IsColliding(Vector2 direction)
    {
        var playerBounds = collider2d.bounds;
        var hit = Physics2D.BoxCast(
            origin: playerBounds.center,
            size: playerBounds.size * collisionBoxSize,
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
}