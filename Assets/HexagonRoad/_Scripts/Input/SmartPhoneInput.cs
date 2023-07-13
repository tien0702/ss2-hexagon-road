using UnityEngine;

public class SmartPhoneInput : MonoBehaviour, IUserInput
{
    public bool Change()
    {
        return false;
    }

    public bool Selected()
    {
        return false;
    }

    public bool TurnLeft()
    {
        return false;
    }

    public bool TurnRight()
    {
        return false;
    }

    public float Zoom()
    {
        return 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
