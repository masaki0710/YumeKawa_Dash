using UnityEngine;

public class PendulumLine : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            line.SetPosition(0, startPoint.position);
            line.SetPosition(1, endPoint.position);
        }
    }
}
