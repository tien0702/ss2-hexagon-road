using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }
    public GameData GData { private set; get; }
    public bool GameOver = false;
    [SerializeField] private Transform gameOverLayer;
    [SerializeField] private List<GameData> modeData = new List<GameData>();

    #region Score
    public static int NumScore = 5;
    public Dictionary<ScoreHighType, List<int>> HighScore { private set; get; }
    #endregion
    private void Awake()
    {
        Instance = this;
        OnStartGame();
    }
    private void Start()
    {
        HexagonGird gird = GameObject.Find("HexagonGird").GetComponent<HexagonGird>();
        gird.Register(EventID.HexGirdEventID.OnNextFace, OnNextFace);
        gird.Register(EventID.HexGirdEventID.OnCreateNewFace, OnCreateNewFace);
        gird.Register(EventID.HexGirdEventID.OnChangeFace, OnChangeFace);
        gird.Register(EventID.HexGirdEventID.OnReturnOrignFace, OnReturnOrigin);
    }

    public bool CanChangeFace()
    {
        return GData.MaxFaces > 0;
    }

    bool LoadData()
    {
        GData = ScriptableObject.CreateInstance<GameData>();
        string content;
        if (File.Exists(GamePath.GDataPath))
        {
            content = File.ReadAllText(GamePath.GDataPath);
            GData = JsonConvert.DeserializeObject<GameData>(content);
            GData.MaxFaces = modeData.Find(d => d.Mode == GData.Mode).MaxFaces;
        }
        if (File.Exists(GamePath.HighScorePath))
        {
            content = File.ReadAllText(GamePath.HighScorePath);
            HighScore = JsonConvert.DeserializeObject<Dictionary<ScoreHighType, List<int>>>(content);
        }
        else
        {
            HighScore = new Dictionary<ScoreHighType, List<int>>();
            foreach (ScoreHighType type in Enum.GetValues(typeof(ScoreHighType)))
            {
                HighScore.Add(type, new List<int>());
            }
        }
        return true;
    }

    void OnStartGame()
    {
        HexagonVertex.InitPoint();
        LoadData();
    }

    void OnNextFace(HexagonFace face)
    {
        ++GData.MaxCombo;
        this.GData.Score += GData.MaxCombo;
        if (face.IsAllVisited()) GData.FullFaceNum++;
        HUDLayer.Instance.ReloadGameInfo();
    }

    void OnCreateNewFace(HexagonFace face)
    {
        this.GData.MaxCombo = 0;
        GData.MaxFaces = Mathf.Max(0, GData.MaxFaces - 1);
        HUDLayer.Instance.ReloadGameInfo();
    }

    void OnChangeFace(HexagonFace face)
    {
        GData.MaxFaces = (int)MathF.Max(0, GData.MaxFaces - 1);
        HUDLayer.Instance.ReloadGameInfo();
    }
    void OnReturnOrigin(HexagonFace face)
    {
        this.GameOver = true;
        this.EndGame();
    }

    public void EndGame()
    {
        UpdateScore();
        gameOverLayer.gameObject.SetActive(true);
    }

    public void UpdateScore()
    {
        HighScore[ScoreHighType.Score].Add(GData.Score);
        HighScore[ScoreHighType.Score].Sort((a, b) => b.CompareTo(a));
        if (HighScore[ScoreHighType.Score].Count > NumScore)
            HighScore[ScoreHighType.Score] = HighScore[ScoreHighType.Score].GetRange(0, NumScore);

        HighScore[ScoreHighType.MaxCombo].Add(GData.MaxCombo);
        HighScore[ScoreHighType.MaxCombo].Sort((a, b) => b.CompareTo(a));
        if (HighScore[ScoreHighType.MaxCombo].Count > NumScore)
            HighScore[ScoreHighType.MaxCombo] = HighScore[ScoreHighType.MaxCombo].GetRange(0, NumScore);

        HighScore[ScoreHighType.FullFace].Add(GData.FullFaceNum);
        HighScore[ScoreHighType.FullFace].Sort((a, b) => b.CompareTo(a));
        if (HighScore[ScoreHighType.FullFace].Count > NumScore)
            HighScore[ScoreHighType.FullFace] = HighScore[ScoreHighType.FullFace].GetRange(0, NumScore);

        string jsoncontent = JsonConvert.SerializeObject(HighScore);
        File.WriteAllText(GamePath.HighScorePath, jsoncontent);
    }
}
