using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class PartyCancelUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image _backgroundSprite;
    [SerializeField] Sprite onSelectBackground;
    [SerializeField] Sprite onDeselectBackground;

    public void OnHandleStart()
    {
        Deselected();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _backgroundSprite.sprite = onSelectBackground;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselected();
    }

    void Deselected()
    {
        _backgroundSprite.sprite = onDeselectBackground;
    }
}
