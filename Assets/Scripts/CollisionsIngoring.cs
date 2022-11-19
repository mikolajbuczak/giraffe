using UnityEngine;

public class CollisionsIngoring : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject head;

    private void Awake()
    {
        var neckCollider = GetComponent<PolygonCollider2D>();
        Physics2D.IgnoreCollision(body.GetComponent<Collider2D>(), neckCollider);
        Physics2D.IgnoreCollision(head.GetComponent<Collider2D>(), neckCollider);
    }
}
