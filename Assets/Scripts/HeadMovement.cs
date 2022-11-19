using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    public GameObject neck;
    public float movementSpeed;

    private Rigidbody2D rb;
    private Vector2 velocity;
    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        Physics2D.IgnoreCollision(neck.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

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
        HandleMovement();
    }

    private void HandleMovement()
    {
        var horizontalVelocity = horizontalInput * movementSpeed;
        var verticalVelocity = verticalInput * movementSpeed;

        velocity = new Vector2(horizontalVelocity, verticalVelocity);

        rb.velocity = velocity;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
}
