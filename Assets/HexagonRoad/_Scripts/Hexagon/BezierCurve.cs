using UnityEngine;

[System.Serializable]
public class BezierCurve
{
    public Vector2 StartPoint;
    public Vector2 EndPoint;
    public Vector2 CtrlPoint1;
    public Vector2 CtrlPoint2;

    public BezierCurve()
    {

    }

    public BezierCurve(Vector2 startPoint, Vector2 endPoint, Vector2 ctrlPoint1, Vector2 ctrlPoint2)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        CtrlPoint1 = ctrlPoint1;
        CtrlPoint2 = ctrlPoint2;
    }

    public Vector2 GetSegment(float time)
    {
        Vector2 pos = new Vector2();

        pos = Mathf.Pow(1 - time, 3) * StartPoint +
            3 * Mathf.Pow(1 - time, 2) * time * CtrlPoint1 +
            3 * (1 - time) * Mathf.Pow(time, 2) * CtrlPoint2 +
        Mathf.Pow(time, 3) * EndPoint;

        return pos;
    }
}
