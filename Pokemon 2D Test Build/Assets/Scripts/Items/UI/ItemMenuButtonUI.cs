using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMenuButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    Image image;
    [SerializeField] itemType itemTypeMenu;
    [SerializeField] Sprite nonSelected;
    [SerializeField] Sprite currentlySelected;
    [SerializeField] GameObject selector;
    [TextArea]
    [SerializeField] string menuDecscription;
    [SerializeField] Text messageBox;

    public void Initialization()
    {
        image = GetComponent<Image>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetMessageText(menuDecscription);
        EnableSelector(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        EnableSelector(false);
    }

    public void ActiveMenu(bool enabled)
    {
        if(enabled == true)
        {
            image.sprite = currentlySelected;
        }
        else
        {
            image.sprite = nonSelected;
        }
    }

    void EnableSelector(bool enabled)
    {
        selector.SetActive(enabled);
    }

    public itemType ItemMenuType
    {
        get { return itemTypeMenu; }
    }

    void SetMessageText(string dialog)
    {
        messageBox.text = dialog;
    }
}
