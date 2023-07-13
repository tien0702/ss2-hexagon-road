using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameButton : Button
{
    protected override void Start()
    {
        base.Start();
        onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        AudioManager.Instance.PlaySFX("OnClick");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        AudioManager.Instance.PlaySFX("HoverSFX");
    }

}
