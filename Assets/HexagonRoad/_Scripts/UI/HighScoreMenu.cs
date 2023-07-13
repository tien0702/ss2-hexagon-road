using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
public enum ScoreHighType
{
    Score,
    MaxCombo,
    FullFace
}

public class HighScoreMenu : MonoBehaviour
{
    public Dictionary<ScoreHighType, List<int>> HighScore { private set; get; }

    TextMeshProUGUI content;
    GameButton scoreBtn;

    private void Awake()
    {
        var table = transform.Find("Table");
        content = table.Find("Content").GetComponent<TextMeshProUGUI>();
        scoreBtn = transform.Find("Sidebar").Find("Score").GetComponent<GameButton>();
        LoadData();
    }

    private void OnEnable()
    {
        scoreBtn.Select();
        ChangeContent((int)ScoreHighType.Score);
    }

    bool LoadData()
    {
        if (File.Exists(GamePath.HighScorePath))
        {
            string content = File.ReadAllText(GamePath.HighScorePath);
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

    public void ChangeContent(int index)
    {
        List<int> scores = HighScore[(ScoreHighType)index];
        string contentText = string.Empty;
        scores.ForEach( s => contentText += (s + "\n"));

        content.text = contentText;
    }
}
