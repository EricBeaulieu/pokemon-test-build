using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowButtonUI : MonoBehaviour, ISelectHandler
{
    [SerializeField] bool right;
    Image image;
    [SerializeField] Sprite nonSelected;
    [SerializeField] Sprite currentlySelected;
    InventorySystem inventory;

    public void Initialization(InventorySystem currentSystem)
    {
        image = GetComponent<Image>();
        inventory = currentSystem;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(FlashToNextPage());
    }

    void EnableSelector(bool enabled)
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

    IEnumerator FlashToNextPage()
    {
        EnableSelector(true);
        inventory.LoadNextPage(right);
        yield return new WaitForSeconds(0.25f);
        EventSystem.current.SetSelectedGameObject(inventory.ReturnToLastButtonPressed());
        yield return new WaitForSeconds(0.75f);
        EnableSelector(false);
    }
}
