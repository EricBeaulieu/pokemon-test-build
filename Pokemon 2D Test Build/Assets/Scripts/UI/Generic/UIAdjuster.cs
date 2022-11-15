using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAdjuster : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] int spacingBetweenEachButton = 5;
    [SerializeField] int paddingTopBottom = 10;
    [SerializeField] List<UIDynamicButton> buttons;
    int buttonHeight;
    int currentButtonCount;

    public void Initialization()
    {
        buttonHeight = (int)buttons[0].GetComponent<RectTransform>().sizeDelta.y;
        //UpdateSizeAccordingToSelection(6);
    }

    public void UpdateSizeAccordingToSelection(int buttonCount)
    {
        currentButtonCount = buttonCount;
        int newHeight = (paddingTopBottom * 2) + (spacingBetweenEachButton * (buttonCount - 1)) + (buttonHeight * buttonCount);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i >= buttonCount)
            {
                buttons[i].gameObject.SetActive(false);
                continue;
            }
            buttons[i].gameObject.SetActive(true);
            newHeight = (paddingTopBottom) + ((spacingBetweenEachButton + buttonHeight) * i);
            Debug.Log($"Button at {i} new height is {newHeight}", buttons[i].gameObject);
            buttons[i].RectTransform.anchoredPosition = new Vector2(Mathf.RoundToInt(buttons[i].RectTransform.anchoredPosition.x), newHeight);
        }

        //Update Naviagtion
        for (int i = 0; i < currentButtonCount; i++)
        {
            if (i == 0)
            {
                //have to pull the variable out, then change it and set it back in
                var navigation = buttons[i].GetButton.navigation;
                navigation.selectOnDown = buttons[currentButtonCount-1].GetButton;
                navigation.selectOnUp = buttons[i+1].GetButton;
                buttons[i].GetButton.navigation = navigation;
            }
            else if( i == currentButtonCount - 1)
            {
                var navigation = buttons[i].GetButton.navigation;
                navigation.selectOnDown = buttons[i - 1].GetButton;
                navigation.selectOnUp = buttons[0].GetButton;
                buttons[i].GetButton.navigation = navigation;
            }
            else
            {
                var navigation = buttons[i].GetButton.navigation;
                navigation.selectOnDown = buttons[i - 1].GetButton;
                navigation.selectOnUp = buttons[i+1].GetButton;
                buttons[i].GetButton.navigation = navigation;
            }
        }
    }

    public UIDynamicButton GetButtonAtPosition(int index)
    {

        if(index >= currentButtonCount)
        {
            return null;
        }
        return buttons[index];
    }

    public GameObject SelectTopButton()
    {
        return buttons[currentButtonCount-1].gameObject;
    }
}
