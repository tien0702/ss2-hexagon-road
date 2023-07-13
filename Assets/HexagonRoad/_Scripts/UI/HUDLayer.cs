using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDLayer : MonoBehaviour
{
    public static HUDLayer Instance { private set; get; }
    public Loader loaderPrefab;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI faceRemaining;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        faceRemaining = transform.Find("Hexagon").Find("FaceRemaining").GetComponent<TextMeshProUGUI>();

        this.ReloadGameInfo();
    }

    public void ReloadGameInfo()
    {
        score.text = GameManager.Instance.GData.Score.ToString();
        faceRemaining.text = GameManager.Instance.GData.MaxFaces.ToString();
    }

    public void BackMainMenu()
    {
        var loader = Instantiate(loaderPrefab);
        loader.LoadSceneByName("MainMenu");
    }
}
