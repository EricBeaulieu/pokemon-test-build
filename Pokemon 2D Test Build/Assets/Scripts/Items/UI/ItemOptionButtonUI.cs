using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOptionButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image standardBackground;
    [SerializeField] GameObject background;
    [SerializeField] GameObject selector;
    [SerializeField] GameObject textBox;
    [SerializeField] Button button;

    public void SetData(Color missingColor,bool showButton)
    {
        button.onClick.RemoveAllListeners();
        standardBackground.color = missingColor;
        background.SetActive(showButton);
        textBox.SetActive(showButton);
    }

    public void OnSelect(BaseEventData eventData)
    {
        EnableSelector(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        EnableSelector(false);
    }

    void EnableSelector(bool enabled)
    {
        selector.SetActive(enabled);
    }

    public Button GetButton
    {
        get { return button; }
    }
}
