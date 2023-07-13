using System.Collections.Generic;
using UnityEngine;

public static class HexagonUtilities
{
    public static readonly float SideLength = 1f;
    public static readonly float SideXRate = 0.75f;
    public static readonly float SideZRate = 0.4330127018922193f;
    public static readonly float DegreesPerStep = 60f;
    public static Vector2 UnitPerEdge(EdgeDirection edge)
    {
        Vector2 pos = Vector2.zero;
        switch (edge)
        {
            case EdgeDirection.AboveLeft:
                pos = new Vector2(-1, 1);
                break;
            case EdgeDirection.AboveMid:
                pos = new Vector2(0, 2);
                break;
            case EdgeDirection.AboveRight:
                pos = new Vector2(1, 1);
                break;
            case EdgeDirection.UnderLeft:
                pos = new Vector2(-1, -1);
                break;
            case EdgeDirection.UnderMid:
                pos = new Vector2(0, -2);
                break;
            case EdgeDirection.UnderRight:
                pos = new Vector2(1, -1);
                break;
        }

        return pos;
    }

    public static EdgeDirection GetEdgeBeside(EdgeDirection edge)
    {
        switch (edge)
        {
            case EdgeDirection.AboveLeft:
                return EdgeDirection.UnderRight;
            case EdgeDirection.AboveMid:
                return EdgeDirection.UnderMid;
            case EdgeDirection.AboveRight:
                return EdgeDirection.UnderLeft;
            case EdgeDirection.UnderRight:
                return EdgeDirection.AboveLeft;
            case EdgeDirection.UnderMid:
                return EdgeDirection.AboveMid;
            case EdgeDirection.UnderLeft:
                return EdgeDirection.AboveRight;
        }

        return EdgeDirection.UnderRight;
    }

    public static GatePosition GetGatePosBeside(EdgeDirection edge, GatePosition position)
    {
        if (edge == EdgeDirection.AboveLeft || edge == EdgeDirection.UnderLeft)
        {
            position = ToggleGatePosition(position);
        }

        return position;
    }

    public static GatePosition ToggleGatePosition(GatePosition position)
    {
        if (position == GatePosition.GateLeft)
        {
            return GatePosition.GateRight;
        }
        else
        {
            return GatePosition.GateLeft;
        }
    }

    public static EdgeDirection GetNextEdge(EdgeDirection edge)
    {
        int nextEdge = (int)edge + 1;
        if (nextEdge > 5) nextEdge = 0;
        return (EdgeDirection)nextEdge;
    }

    public static EdgeDirection GetBackEdge(EdgeDirection edge)
    {
        int backEdge = (int)edge - 1;
        if (backEdge < 0) backEdge = 5;
        return (EdgeDirection)backEdge;
    }

    public static bool CheckOriginPos(Vector2 pos)
    {
        return ((int)pos.x == 0 && (int)pos.y == 0);
    }


    public static Gate RandomGate(List<EdgeDirection> edgeHasGate, Dictionary<EdgeDirection, bool[]> representEdges)
    {
        Gate gate = new Gate();
        gate.OnEdge = RandomEdge(edgeHasGate);
        gate.Position = RandomGatePosition(representEdges[gate.OnEdge]);
        representEdges[gate.OnEdge][(int)gate.Position] = true;

        return gate;
    }

    public static EdgeDirection RandomEdge(List<EdgeDirection> edgesLst)
    {
        return edgesLst[UnityEngine.Random.Range(0, edgesLst.Count)];
    }

    public static GatePosition RandomGatePosition(bool[] values)
    {
        return (GatePosition)(values[0] ? (1) : 0);
    }
}
