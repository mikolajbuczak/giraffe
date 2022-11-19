using System;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    public GameObject neck;
    public Transform neckOrigin;
    public float movementSpeed;
    public float minDistance = 2f;
    public float maxDistance = 10f;

    private Rigidbody2D rb;
    private Vector2 velocity;
    private float horizontalInput;
    private float verticalInput;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var otherRb = collision.collider.GetComponent<Rigidbody2D>();

        if (otherRb == null)
        {
            return;
        }

        if (otherRb.CompareTag("Body"))
        {
            otherRb.velocity = Vector2.zero;
            otherRb.angularVelocity = 0f;
        }
    }
}
