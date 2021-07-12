using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ItemCancelUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image backgroundSprite;
    [SerializeField] Sprite onSelectBackground;
    [SerializeField] Sprite onDeselectBackground;

    public void OnHandleStart()
    {
        Deselected();
    }

    public void OnSelect(BaseEventData eventData)
    {
        backgroundSprite.sprite = onSelectBackground;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselected();
    }

    void Deselected()
    {
        backgroundSprite.sprite = onDeselectBackground;
    }
}
