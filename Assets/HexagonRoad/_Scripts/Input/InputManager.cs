using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public IUserInput UserInput;
    [SerializeField] private WindowsInput winInput;
    [SerializeField] private SmartPhoneInput smartPhoneInput;

    private void Awake()
    {
        Instance = this;
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            UserInput = Instantiate(winInput, transform);
        else if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            UserInput = Instantiate(smartPhoneInput, transform);
    }
}
