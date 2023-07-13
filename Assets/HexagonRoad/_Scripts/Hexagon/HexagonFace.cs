using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class HexagonFace : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;

    public int XPos, YPos;
    Dictionary<EdgeDirection, Road[]> Edges = new Dictionary<EdgeDirection, Road[]>();
    List<Road> roadLst = new List<Road>();
    List<Gate> gateInFace = new List<Gate>();


    // rotation 
    Animator animator;
    [SerializeField] private float timeRotate = 1.5f;
    [SerializeField] private float smoothRate = 0.25f;
    bool isRotating = false;
    bool canRotate = false;
    float currentTime;
    float angleOffset;
    float timeAppear = 0.35f;
    private void Awake()
    {
        foreach (EdgeDirection ed in Enum.GetValues(typeof(EdgeDirection)))
        {
            Edges.Add(ed, new Road[Enum.GetNames(typeof(GatePosition)).Length]);
        }
        InitRoads();
        AudioManager.Instance.PlaySFX("Appear");
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.DOScale(1, timeAppear);

        var rotate = transform.localRotation;
        transform.localRotation = new Quaternion(0, 0, 10, 0);
        transform.DORotate(rotate.eulerAngles, timeAppear, RotateMode.Fast);
        StartCoroutine(LockRotate(timeAppear));
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsAllVisited() => !(roadLst.Any(r => !r.IsVisited));

    public void TurnLeft()
    {
        if (!canRotate) return;
        var temp = new Dictionary<EdgeDirection, Road[]>();
        foreach (EdgeDirection edge in Enum.GetValues(typeof(EdgeDirection)))
        {
            var roads = Edges[HexagonUtilities.GetNextEdge(edge)];
            temp.Add(edge, roads);
        }

        gateInFace.ForEach(g => g.TurnLeft());
        Edges = temp;
        angleOffset += HexagonUtilities.DegreesPerStep;
        currentTime = 0f;
        if (!isRotating) StartCoroutine(Rotate());
        AudioManager.Instance.PlaySFX("Rotate");
    }

    public void TurnRight()
    {
        if (!canRotate) return;
        var temp = new Dictionary<EdgeDirection, Road[]>();
        foreach (EdgeDirection edge in Enum.GetValues(typeof(EdgeDirection)))
        {
            var roads = Edges[HexagonUtilities.GetBackEdge(edge)];
            temp.Add(edge, roads);
        }

        gateInFace.ForEach(g => g.TurnRight());
        Edges = temp;

        angleOffset -= HexagonUtilities.DegreesPerStep;
        currentTime = 0f;
        if(!isRotating) StartCoroutine(Rotate());
        AudioManager.Instance.PlaySFX("Rotate");
    }

    private IEnumerator Rotate()
    {
        isRotating = true;
        while(currentTime < timeRotate)
        {
            float angle = Mathf.Lerp(0f, angleOffset, currentTime / timeRotate) * smoothRate;
            angleOffset -= angle;
            transform.Rotate(new Vector3(0, 0, angle));
            currentTime += Time.deltaTime;
            yield return null;
        }
        isRotating = false;
        currentTime = 0f;
    }

    public Road GetRoad(EdgeDirection edge, GatePosition gatePosition)
    {
        return Edges[edge][(int)gatePosition];
    }

    void InitRoads()
    {
        var representEdges = new Dictionary<EdgeDirection, bool[]>();
        foreach (EdgeDirection ed in Enum.GetValues(typeof(EdgeDirection)))
        {
            representEdges.Add(ed, new bool[2]);
        }

        List<EdgeDirection> edgeHasGate = new List<EdgeDirection>(Edges.Keys);

        while (edgeHasGate.Count > 0)
        {
            // random gate enter and exit of road
            Gate[] gates = new Gate[Enum.GetNames(typeof(GateType)).Length];

            gates[(int)GateType.Enter] = HexagonUtilities.RandomGate(edgeHasGate, representEdges);
            UpdateGateInEdge(gates[(int)GateType.Enter].OnEdge, representEdges[gates[(int)GateType.Enter].OnEdge], edgeHasGate);

            gates[(int)GateType.Exit] = HexagonUtilities.RandomGate(edgeHasGate, representEdges);
            UpdateGateInEdge(gates[(int)GateType.Exit].OnEdge, representEdges[gates[(int)GateType.Exit].OnEdge], edgeHasGate);

            // init road and set gates from gate random
            var road = Instantiate(RoadPrefab, transform).GetComponent<Road>();
            road.DrawRoad(gates[(int)GateType.Enter], gates[(int)GateType.Exit]);
            road.FaceOwner = this;
            road.GatesType[(int)GateType.Enter] = gates[(int)GateType.Enter];
            road.GatesType[(int)GateType.Exit] = gates[(int)GateType.Exit];
            roadLst.Add(road);

            // add gate to gateInFace
            gates.ToList().ForEach(g =>
            {
                UpdateEdges(g, road);
                gateInFace.Add(g);
            });
        }
    }

    void UpdateGateInEdge(EdgeDirection edgeCheck, bool[] gateSlot, List<EdgeDirection> edgeHasGate)
    {
        if (gateSlot.Any(g => g == false)) return;
        edgeHasGate.Remove(edgeCheck);
    }

    void UpdateEdges(Gate gate, Road road)
    {
        EdgeDirection edge = gate.OnEdge;
        GatePosition gatePosition = gate.Position;
        Edges[edge][(int)gatePosition] = road;
    }

    IEnumerator LockRotate(float time)
    {
        canRotate = false;
        yield return new WaitForSeconds(time);
        canRotate = true;
    }

}
