using UnityEngine;
using System;
using System.IO;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { private set; get; }

    [SerializeField] private Sound[] SFXSounds;
    [SerializeField] private Sound[] MusicSounds;

    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;

    public GameOptions OptionsData { private set; get; }

    void Awake()
    {
        MusicSource = transform.Find("BGM_Source").GetComponent<AudioSource>();
        SFXSource = transform.Find("SFX_Source").GetComponent<AudioSource>();
        if (Instance == null || Instance != this)
        {
            Instance = this;
            if (!File.Exists(GameOptions.GetPath()))
            {
                OptionsData = ScriptableObject.CreateInstance<GameOptions>();
                GameOptions.SaveToFile(OptionsData);
            }
            else
            {
                OptionsData = GameOptions.LoadFromFile();
            }
        }
    }

    private void Start()
    {
        MusicSource.volume = OptionsData.BGM_Volume;
        SFXSource.volume = OptionsData.SFX_Volume;

        MusicSource.mute = OptionsData.IsMuteMusic;
        SFXSource.mute = OptionsData.IsMuteSFX;
        PlayMusic("BGM1", true);

    }

    public void PlayMusic(string name, bool loop)
    {
        Sound sound = Array.Find(MusicSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.Log(string.Format("Sound {0} Not Found", name));
        }
        else
        {
            MusicSource.clip = sound.clip;
            MusicSource.loop = loop;
            MusicSource.Play();
        }
    }

    public void StopMusic(string name)
    {
        Sound sound = Array.Find(MusicSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.Log(string.Format("Sound {0} Not Found", name));
        }
        else
        {
            MusicSource?.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(SFXSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.Log(string.Format("SFX {0} Not Found", name));
        }
        else
        {
            SFXSource.PlayOneShot(sound.clip);
        }
    }

    public void SetMusicVolume(float value)
    {
        OptionsData.BGM_Volume = value;
        MusicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        OptionsData.SFX_Volume = value;
        SFXSource.volume = value;
    }
    public void ToggleMusic()
    {
        OptionsData.IsMuteMusic = !OptionsData.IsMuteMusic;
        MusicSource.mute = OptionsData.IsMuteMusic;
    }
    public void ToggleSFX()
    {
        OptionsData.IsMuteSFX = !OptionsData.IsMuteSFX;
        SFXSource.mute = OptionsData.IsMuteSFX;
    }

    public void SaveGameOptions()
    {
        GameOptions.SaveToFile(OptionsData);
    }
}
