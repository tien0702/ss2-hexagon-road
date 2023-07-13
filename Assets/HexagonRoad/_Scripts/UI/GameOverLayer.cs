using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverLayer : MonoBehaviour
{
    [SerializeField] private Loader loaderPrefab;

    TextMeshProUGUI score, maxCombo, fullFace;

    private void Awake()
    {
        var table = transform.Find("Table");
        score = table.Find("Score").GetComponent<TextMeshProUGUI>();
        maxCombo = table.Find("MaxCombo").GetComponent<TextMeshProUGUI>();
        fullFace = table.Find("FullFace").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            var loader = Instantiate(loaderPrefab);
            loader.LoadSceneByName("MainMenu");
        }
    }

    private void OnEnable()
    {
        var data = GameManager.Instance.GData;
        score.text = "Score: " + data.Score.ToString();
        maxCombo.text = "Max Combo: " + data.MaxCombo.ToString();
        fullFace.text = "Full Face: " + data.FullFaceNum.ToString();
    }
}
