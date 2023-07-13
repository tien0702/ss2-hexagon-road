using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] private float minOrthographicSize = 2.5f;
    [SerializeField] private float maxOrthographicSize = 4.0f;
    [SerializeField] private float incrRate = 0.2f;
    CinemachineVirtualCamera virtualCamera;
    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        float zoom = InputManager.Instance.UserInput.Zoom();
        if(zoom > 0)
        {
            virtualCamera.m_Lens.OrthographicSize = MathF.Max(virtualCamera.m_Lens.OrthographicSize - incrRate, minOrthographicSize);
        }
        else if(zoom < 0)
        {
            virtualCamera.m_Lens.OrthographicSize = MathF.Min(virtualCamera.m_Lens.OrthographicSize + incrRate, maxOrthographicSize);
        }
    }
}
