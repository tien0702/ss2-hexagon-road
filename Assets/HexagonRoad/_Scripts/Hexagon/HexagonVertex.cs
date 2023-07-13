using System;
using System.Collections.Generic;
using UnityEngine;

public static class HexagonVertex
{
    public static bool IsInit = false;
    public static readonly Vector2[] Vertexs = new Vector2[]
    {
        new Vector2(-0.5f, 0f),
        new Vector2(-0.25f, 0.4330127018922193f),
        new Vector2(0.25f, 0.4330127018922193f),
        new Vector2(0.5f, 0f),
        new Vector2(0.25f, -0.4330127018922193f),
        new Vector2(-0.25f, -0.4330127018922193f)
    };

    public static readonly Dictionary<EdgeDirection, Vector2[]> AllGates = new Dictionary<EdgeDirection, Vector2[]>();

    public static void InitPoint()
    {
        if (IsInit) return;
        IsInit = true;
        AllGates.Add(EdgeDirection.AboveLeft, PointsInEdge(HexagonVertex.Vertexs[0], HexagonVertex.Vertexs[1]));
        AllGates.Add(EdgeDirection.AboveMid, PointsInEdge(HexagonVertex.Vertexs[1], HexagonVertex.Vertexs[2]));
        AllGates.Add(EdgeDirection.AboveRight, PointsInEdge(HexagonVertex.Vertexs[2], HexagonVertex.Vertexs[3]));
        AllGates.Add(EdgeDirection.UnderRight, PointsInEdge(HexagonVertex.Vertexs[3], HexagonVertex.Vertexs[4]));
        AllGates.Add(EdgeDirection.UnderMid, PointsInEdge(HexagonVertex.Vertexs[4], HexagonVertex.Vertexs[5]));
        AllGates.Add(EdgeDirection.UnderLeft, PointsInEdge(HexagonVertex.Vertexs[5], HexagonVertex.Vertexs[0]));
    }

    static Vector2[] PointsInEdge(Vector2 A, Vector2 B)
    {
        Vector2[] points = new Vector2[2];

        float x1 = (3 * A.x + B.x) / 4f;
        float y1 = (3 * A.y + B.y) / 4f;

        float x2 = (A.x + 3 * B.x) / 4f;
        float y2 = (A.y + 3 * B.y) / 4f;

        points[0] = new Vector2(x1, y1);
        points[1] = new Vector2(x2, y2);

        return points;
    }

}
