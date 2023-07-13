using UnityEngine;

public class Gate
{
    public EdgeDirection OnEdge;
    public GatePosition Position;

    public void TurnLeft()
    {
        OnEdge = HexagonUtilities.GetBackEdge(OnEdge);
    }

    public void TurnRight()
    {
        OnEdge = HexagonUtilities.GetNextEdge(OnEdge);
    }
}
