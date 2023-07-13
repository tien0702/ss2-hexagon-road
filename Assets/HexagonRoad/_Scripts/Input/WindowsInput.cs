using UnityEngine;
using UnityEngine.EventSystems;

public class WindowsInput : MonoBehaviour, IUserInput
{
    bool isTurnLeft, isTurnRight, isSelected, isChange;
    float zoomDis;
    void Update()
    {
        isTurnLeft = false;
        isTurnRight = false;
        isSelected = false;
        isChange = false;
        zoomDis = 0f;
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0)
        {
            isTurnLeft = true;
        }
        else if (scrollInput < 0)
        {
            isTurnRight = true;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSelected = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                isChange = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            zoomDis = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            zoomDis = -1f;
        }
    }
    public bool Change()
    {
        return isChange;
    }

    public bool Selected()
    {
        return isSelected;
    }

    public bool TurnLeft()
    {
        return isTurnLeft;
    }

    public bool TurnRight()
    {
        return isTurnRight;
    }

    public float Zoom()
    {
        return zoomDis;
    }
}
