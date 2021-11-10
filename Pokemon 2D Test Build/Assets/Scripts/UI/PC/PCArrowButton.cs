using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCArrowButton : MonoBehaviour,ISelectHandler
{
    [SerializeField] bool right;
    Image image;
    [SerializeField] PCSystem pCSystem;
    [SerializeField] Sprite nonSelected;
    [SerializeField] Sprite currentlySelected;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(FlashToNextPage());
    }

    void EnableSelector(bool enabled)
    {
        if (enabled == true)
        {
            image.sprite = currentlySelected;
        }
        else
        {
            image.sprite = nonSelected;
        }
    }

    IEnumerator FlashToNextPage()
    {
        EnableSelector(true);
        yield return new WaitForSeconds(0.01f);
        yield return pCSystem.SelectNewBox(right);
        EnableSelector(false);
    }
}
