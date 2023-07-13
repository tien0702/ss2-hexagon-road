using Assets.SimpleLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private Color visitedColor = new Color();
    public bool IsVisited { set; get; }
    public Gate[] GatesType;
    public HexagonFace FaceOwner;
    public BezierCurve Curve;
    public float CtrlRate = 0.3f;

    [SerializeField][Range(0f, 1f)] private float timeIncrRate = 0.01f;
    // Components
    LineRenderer lineRenderer;
    List<Vector3> points = new List<Vector3>();
    int pointCounts = 0;

    private void Awake()
    {
        GatesType = new Gate[Enum.GetNames(typeof(GateType)).Length];
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetVisit(bool isVisit)
    {
        this.IsVisited = isVisit;

        float alpha = 1f;
        var line = GetComponent<LineRenderer>();
        var gradient = new Gradient();
        if (IsVisited)
        {
            gradient.SetKeys(
                 new GradientColorKey[] { new GradientColorKey(visitedColor, 0.0f), new GradientColorKey(visitedColor, 1.0f) },
                  new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
            line.colorGradient = gradient;
        }
        else
        {
            gradient.SetKeys(
                 new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                  new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
            line.startColor = Color.white;
            line.endColor = Color.white;
        }
    }

    public void SetGate(GateType type, GatePosition position, EdgeDirection edge)
    {
        GatesType[(int)type].Position = position;
        GatesType[(int)type].OnEdge = edge;
    }
    public void DrawRoad(Gate startRoad, Gate endRoad)
    {
        this.Curve = GetCurve(new Tuple<EdgeDirection, GatePosition>(startRoad.OnEdge, startRoad.Position), new Tuple<EdgeDirection, GatePosition>(endRoad.OnEdge, endRoad.Position));
        ResetLine();
        DrawWithBezierCurve();
    }

    BezierCurve GetCurve(Tuple<EdgeDirection, GatePosition> startRoad, Tuple<EdgeDirection, GatePosition> endRoad)
    {
        BezierCurve curve = new BezierCurve();
        curve.StartPoint = HexagonVertex.AllGates[startRoad.Item1][(int)startRoad.Item2];
        curve.EndPoint = HexagonVertex.AllGates[endRoad.Item1][(int)endRoad.Item2];

        curve.CtrlPoint1 = curve.StartPoint * CtrlRate;
        curve.CtrlPoint2 = curve.EndPoint * CtrlRate;

        return curve;
    }

    void ResetLine()
    {
        pointCounts = 0;
        lineRenderer.positionCount = 0;
        points.Clear();
    }

    void DrawWithBezierCurve()
    {
        for (float t = 0; t < 1f; t += timeIncrRate)
        {
            Vector2 newPosition = Curve.GetSegment(t);
            this.AddPoint(transform.InverseTransformPoint(newPosition));
        }
    }

    void AddPoint(Vector2 newPoint)
    {
        points.Add(newPoint);
        pointCounts++;

        lineRenderer.positionCount = pointCounts;
        lineRenderer.SetPosition((int)MathF.Max(0, pointCounts - 1), newPoint);
    }

    public void Reverse()
    {
        Gate temp = GatesType[(int)GateType.Enter];
        GatesType[(int)GateType.Enter] = GatesType[(int)GateType.Exit];
        GatesType[(int)GateType.Exit] = temp;

        Vector2 vecTemp = this.Curve.StartPoint;
        this.Curve.StartPoint = this.Curve.EndPoint;
        this.Curve.EndPoint = vecTemp;

        points.Reverse();
        lineRenderer.positionCount = pointCounts;
        lineRenderer.SetPositions(points.ToArray());
    }
}
