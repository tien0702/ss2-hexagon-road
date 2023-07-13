using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexagonGird : MonoBehaviour
{
    #region Observer
    Dictionary<EventID.HexGirdEventID, Action<HexagonFace>> observers = new Dictionary<EventID.HexGirdEventID, Action<HexagonFace>>();

    public void Register(EventID.HexGirdEventID eventID, Action<HexagonFace> callback)
    {
        if (!observers.ContainsKey(eventID))
        {
            observers.Add(eventID, null);
        }
        observers[eventID] += callback;
    }
    public void UnRegister(EventID.HexGirdEventID eventID, Action<HexagonFace> callback)
    {
        if (!observers.ContainsKey(eventID))
        {
            Debug.Log(string.Format("Can't Unregister because HealthEventID {0} not exists!", eventID.ToString()));
            return;
        }
        observers[eventID] -= callback;
    }
    public void PostEvent(EventID.HexGirdEventID eventID, HexagonFace param)
    {
        if (!observers.ContainsKey(eventID))
        {
            Debug.Log(string.Format("Can't PostEvent because HealthEventID {0} not exists!", eventID.ToString()));
            return;
        }

        observers[eventID]?.Invoke(param);
    }
    #endregion

    [SerializeField] private GameObject hexagonFacePrefab;
    [SerializeField] private GameObject hexagonOriginFacePrefab;
    [SerializeField] private BrightSpot brightSpot;

    List<HexagonFace> gird = new List<HexagonFace>();
    HexagonFace newFace;

    EdgeDirection edgeFirst;

    private void Start()
    {
        // init origin hexagon
        var originH = Instantiate(hexagonOriginFacePrefab, transform);
        originH.transform.position = Vector2.zero;

        // random first hexagon
        edgeFirst = (EdgeDirection)(UnityEngine.Random.Range(0, 5));
        var posFirst = GetPosFaceBeside(0, 0, edgeFirst);
        newFace = CreateHexagon((int)posFirst.x, (int)posFirst.y);

        // register bright spot
        brightSpot.Register(EventID.BrightSpotEventID.OnEndMove, OnBrightSpotMoveEnd);
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver) return;
        if (newFace == null) return;

        if (InputManager.Instance.UserInput.TurnLeft())
        {
            newFace.TurnLeft();
        }
        else if (InputManager.Instance.UserInput.TurnRight())
        {
            newFace.TurnRight();
        }
        else if (InputManager.Instance.UserInput.Selected())
        {
            InsertHexagonFace(newFace);
            SpotMoveToFace(newFace);
            newFace = null;
        }
        else if (InputManager.Instance.UserInput.Change() && GameManager.Instance.CanChangeFace())
        {
            Destroy(newFace.gameObject);
            newFace = CreateHexagon(newFace.XPos, newFace.YPos);
            PostEvent(EventID.HexGirdEventID.OnChangeFace, newFace);
        }
    }

    public HexagonFace CreateHexagon(int xPos, int yPos)
    {
        var hexagonFace = Instantiate(hexagonFacePrefab, transform).GetComponent<HexagonFace>();
        hexagonFace.transform.position = new Vector3(xPos * HexagonUtilities.SideLength * HexagonUtilities.SideXRate,
            yPos * HexagonUtilities.SideLength * HexagonUtilities.SideZRate, 0);
        hexagonFace.XPos = xPos;
        hexagonFace.YPos = yPos;
        return hexagonFace;
    }

    void InsertHexagonFace(HexagonFace face)
    {
        if (face == null)
        {
            Debug.Log("Can't add new face, because face is null!");
            return;
        }

        gird.Add(face);
    }

    public HexagonFace GetFaceBeside(int xPos, int yPos, EdgeDirection edge)
    {
        int nextX, nextY;
        Vector2 nextIndex = GetPosFaceBeside(xPos, yPos, edge);
        nextX = (int)nextIndex.x;
        nextY = (int)nextIndex.y;
        HexagonFace face = gird.Find(f => (f.XPos == nextX && f.YPos == nextY));
        return face;
    }

    public Vector2 GetPosFaceBeside(int xPos, int yPos, EdgeDirection edge)
    {
        Vector2 beside = HexagonUtilities.UnitPerEdge(edge);
        Vector2 position;
        position.x = beside.x + xPos;
        position.y = beside.y + yPos;
        return position;
    }

    void OnBrightSpotMoveEnd()
    {
        var exitGate = brightSpot.OnRoad.GatesType[(int)GateType.Exit];
        var currentFace = brightSpot.OnRoad.FaceOwner;
        Vector2 nextPos = GetPosFaceBeside(currentFace.XPos, currentFace.YPos, exitGate.OnEdge);

        if (HexagonUtilities.CheckOriginPos(nextPos))
        {
            this.PostEvent(EventID.HexGirdEventID.OnReturnOrignFace, null);
            return;
        }

        HexagonFace nextFace = GetFaceBeside(currentFace.XPos, currentFace.YPos, exitGate.OnEdge);

        if (nextFace == null)
        {
            if (!GameManager.Instance.CanChangeFace())
            {
                GameManager.Instance.EndGame();
                return;
            }
            nextFace = CreateHexagon((int)nextPos.x, (int)nextPos.y);
            newFace = nextFace;
            this.PostEvent(EventID.HexGirdEventID.OnCreateNewFace, nextFace);
            return;
        }
        SpotMoveToFace(nextFace);
    }

    void SpotMoveToFace(HexagonFace nextFace)
    {
        Gate exitGate = GetSpotExitGate();
        EdgeDirection edgeBeside = HexagonUtilities.GetEdgeBeside(exitGate.OnEdge);
        GatePosition gatePosBeside = HexagonUtilities.ToggleGatePosition(exitGate.Position);
        Road road = nextFace.GetRoad(edgeBeside, gatePosBeside);
        Gate enterGate = road.GatesType[(int)GateType.Enter];
        if (!(enterGate.OnEdge == edgeBeside && enterGate.Position == gatePosBeside)) road.Reverse();
        brightSpot.MoveByRoad(road);
        this.PostEvent(EventID.HexGirdEventID.OnNextFace, nextFace);
    }

    Gate GetSpotExitGate()
    {
        Gate exitGate = null;
        if (brightSpot.OnRoad == null)
        {
            exitGate = new Gate();
            exitGate.OnEdge = edgeFirst;
            exitGate.Position = GatePosition.GateRight;
        }
        else
        {
            exitGate = brightSpot.OnRoad.GatesType[(int)GateType.Exit];
        }

        return exitGate;
    }
}
