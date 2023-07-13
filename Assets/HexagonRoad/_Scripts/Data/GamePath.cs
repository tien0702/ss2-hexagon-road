using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class GamePath
{
    public static string GDataPath = Application.persistentDataPath + "/HexagonRoad/Data/game-data.json";
    public static string GOptionPath = Application.persistentDataPath + "/HexagonRoad/Data/options-data.json";
    public static string HighScorePath = Application.persistentDataPath + "/HexagonRoad/Data/high-score-data.json";

    public static void WriteToFile(string path, object ob)
    {
        string jsoncontent = JsonConvert.SerializeObject(ob);
        File.WriteAllText(path, jsoncontent);
    }
}
