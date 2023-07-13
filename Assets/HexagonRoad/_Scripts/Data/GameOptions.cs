using Newtonsoft.Json;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Option", menuName = "Game Option")]
public class GameOptions : ScriptableObject
{
    public string Language = "English";
    [Range(0f, 1f)] public float BGM_Volume;
    [Range(0f, 1f)] public float SFX_Volume;
    public bool IsMuteMusic;
    public bool IsMuteSFX;
    public static string GetPath()
    {
        return Application.persistentDataPath + "/HexagonRoad/Data/options-data.json";
    }

    public static GameOptions LoadFromFile()
    {
        string content = File.ReadAllText(GameOptions.GetPath());
        var config = JsonConvert.DeserializeObject<GameOptions>(content);
        return config;
    }

    public static void SaveToFile(GameOptions config)
    {
        string jsoncontent = JsonConvert.SerializeObject(config);
        File.WriteAllText(GameOptions.GetPath(), jsoncontent);
    }
}
