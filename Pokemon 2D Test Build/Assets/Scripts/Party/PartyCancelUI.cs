using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class PartyCancelUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    Image _backgroundSprite;

    [SerializeField] Sprite onSelectBackground;
    [SerializeField] Sprite onDeselectBackground;

    void Awake()
    {
        _backgroundSprite = GetComponent<Image>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _backgroundSprite.sprite = onSelectBackground;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _backgroundSprite.sprite = onDeselectBackground;
    }
}