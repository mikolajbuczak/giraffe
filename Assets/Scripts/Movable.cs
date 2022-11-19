using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] LayerMask blockingLayer;
    [SerializeField, Range(0.01f, 1f)] float movementSpeed = 0.04f;

    Vector3 targetPosition = Vector3.zero;

    void Start()
    {
        targetPosition = transform.position;
    }

    void FixedUpdate()
    {
        Move();
    }

    protected void SetTargetPosition(Vector3 direction)
    {
        var boxCast = Physics2D.Raycast(transform.position, direction, 1f, blockingLayer);

        if (boxCast || transform.position != targetPosition)
        {
            return;
        }

        targetPosition = transform.position + direction;
    }

    void Move()
    {
        if (transform.position != targetPosition)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, targetPosition, movementSpeed);
        }
    }
}
