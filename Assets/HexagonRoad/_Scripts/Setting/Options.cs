using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Dropdown language;

    private void Start()
    {
        musicSlider = transform.Find("Table").Find("BGMusic").GetComponent<Slider>();
        sfxSlider = transform.Find("Table").Find("SFXMusic").GetComponent<Slider>();
        language = transform.Find("Table").Find("Language").GetComponent<TMP_Dropdown>();

        int index = language.options.FindIndex(o => o.text == AudioManager.Instance.OptionsData.Language);
        language.value = index;

        sfxSlider.value = AudioManager.Instance.OptionsData.SFX_Volume;
        musicSlider.value = AudioManager.Instance.OptionsData.BGM_Volume;
        language.onValueChanged.AddListener(ChangeLanguage);
    }

    private void OnDestroy()
    {
        language.onValueChanged.RemoveListener(ChangeLanguage);
    }

    public void OnChangeSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
    }
    public void OnChangeBGMVolume()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
    }

    public void OnBack()
    {
        AudioManager.Instance.SaveGameOptions();
    }

    public void ChangeLanguage(int index)
    {
        string languageText = language.options[index].text;
        LocalizationManager.Language = languageText;
        AudioManager.Instance.OptionsData.Language = languageText;
    }

}
