using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] List<Transform> nodes;
    LineRenderer lr;
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = nodes.Count;
    }

    private void Update()
    {
        lr.SetPositions(nodes.ConvertAll(n => n.position - new Vector3(0, 0, 5)).ToArray());
    }

    public Vector3[] GetPositions()
    {
        var positions = new Vector3[lr.positionCount];
        lr.GetPositions(positions);
        return positions;
    }

    public float GetWidth()
    {
        return lr.startWidth;
    }
}
