using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingNumber : MonoBehaviour
{
    public static FloatingNumber Instance { private set; get; }
    [SerializeField] private TextMeshProUGUI floatNumberPrefab;
    BrightSpot Spot;

    private void Start()
    {
        Spot = GameObject.FindObjectOfType<BrightSpot>();
        Spot.Register(EventID.BrightSpotEventID.OnStartMove, OnStartMove);
    }

    public void OnStartMove()
    {
        var floatNum = Instantiate(floatNumberPrefab, transform);
        floatNum.GetComponent<TextMeshProUGUI>().text = "+" + (GameManager.Instance.GData.MaxCombo + 1).ToString();
        floatNum.transform.position = Spot.transform.position;
        floatNum.DOFade(1f, 0.5f).OnComplete(() =>
            floatNum.DOFade(0f, 0.5f).OnComplete(() =>
            Destroy(floatNum.gameObject)
            )
        );
    }
}
