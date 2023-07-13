using Assets.SimpleLocalization;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    GameData gameData;
    [SerializeField] private Loader loaderPrefab;
    private void Awake()
    {
        LocalizationManager.Read();
        LocalizationManager.Language = GameOptions.LoadFromFile().Language;
    }
    void Start()
    {
        if (!File.Exists(GamePath.GDataPath))
        {
            gameData = ScriptableObject.CreateInstance<GameData>();
            string jsoncontent = JsonConvert.SerializeObject(gameData);
            File.WriteAllText(GamePath.GDataPath, jsoncontent);
        }
        else
        {
            string content = File.ReadAllText(GamePath.GDataPath);
            gameData = JsonConvert.DeserializeObject<GameData>(content);
        }
    }

    public void PlayGame()
    {
        var loader = Instantiate(loaderPrefab);
        loader.LoadSceneByName("GameScene");
    }

    public void SwitchRelaxMode()
    {
        gameData.Mode = GameMode.RelaxMode;
        string jsoncontent = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(GamePath.GDataPath, jsoncontent);

        PlayGame();
    }
    public void SwitchChanllengeMode()
    {
        gameData.Mode = GameMode.ChangllengeMode;
        string jsoncontent = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(GamePath.GDataPath, jsoncontent);

        PlayGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
