using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeckCollision : MonoBehaviour
{
    LineController lc;
    PolygonCollider2D polygonCollider;
    List<Vector2> colliderPoints = new List<Vector2>();
    void Start()
    {
        lc = GetComponent<LineController>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        colliderPoints = CalculateColliderPoints();
        polygonCollider.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
    }

    private List<Vector2> CalculateColliderPoints()
    {
        var positions = lc.GetPositions();

        var width = lc.GetWidth();

        var m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
        var deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        var deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        var offsets = new Vector3[2];
        if (positions[1].x == positions[0].x)
        {
            offsets[0] = new Vector2(-(width / 2.0f), 0);
            offsets[1] = new Vector2(width / 2.0f, 0);

        }
        else
        {
            offsets[0] = new Vector3(-deltaX, deltaY);
            offsets[1] = new Vector3(deltaX, -deltaY);
        }

        var colliderPositions = new List<Vector2>
        {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1]
        };

        return colliderPositions;
    }
}
