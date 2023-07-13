using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;

public class TMProLocalized : MonoBehaviour
{
    public string LocalizationKey;

    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        LocalizationManager.LocalizationChanged += Localize;
        Localize();
    }    

    void OnDestroy()
    {
        LocalizationManager.LocalizationChanged -= Localize;
    }

    void Localize()
    {
        textMeshPro.text = LocalizationManager.Localize(LocalizationKey);
    }
}
