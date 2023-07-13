using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightSpot : MonoBehaviour
{
    Dictionary<EventID.BrightSpotEventID, Action> observers = new Dictionary<EventID.BrightSpotEventID, Action>();
    public Road OnRoad;
    public float SpeedModifier;

    public void MoveByRoad(Road road)
    {
        this.OnRoad = road;
        road.SetVisit(true);
        transform.parent = road.transform;
        var lineRenderer = road.GetComponent<LineRenderer>();
        StartCoroutine(StartMove(lineRenderer));
    }
    IEnumerator StartMove(LineRenderer road)
    {
        PostEvent(EventID.BrightSpotEventID.OnStartMove);
        int soundIndex = (GameManager.Instance.GData.MaxCombo) % 9 + 1;
        AudioManager.Instance.PlaySFX(soundIndex.ToString());
        Vector3[] positions = new Vector3[road.positionCount];
        road.GetPositions(positions);

        for(int i = 0; i < positions.Length; ++i)
        {
            transform.localPosition = positions[i];
            yield return new WaitForSeconds(SpeedModifier);
        }
        PostEvent(EventID.BrightSpotEventID.OnEndMove);
    }
    public void Register(EventID.BrightSpotEventID eventID, Action callback)
    {
        if (!observers.ContainsKey(eventID))
        {
            observers.Add(eventID, null);
        }
        observers[eventID] += callback;
    }
    public void UnRegister(EventID.BrightSpotEventID eventID, Action callback)
    {
        if (!observers.ContainsKey(eventID))
        {
            Debug.Log(string.Format("Can't Unregister because HealthEventID {0} not exists!", eventID.ToString()));
            return;
        }
        observers[eventID] -= callback;
    }
    public void PostEvent(EventID.BrightSpotEventID eventID)
    {
        if (!observers.ContainsKey(eventID))
        {
            Debug.Log(string.Format("Can't PostEvent because HealthEventID {0} not exists!", eventID.ToString()));
            return;
        }

        observers[eventID]?.Invoke();
    }
}
