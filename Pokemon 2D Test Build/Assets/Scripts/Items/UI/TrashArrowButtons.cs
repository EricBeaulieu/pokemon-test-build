using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashArrowButtons : MonoBehaviour, ISelectHandler
{
    [SerializeField] bool up;
    Image image;
    InventorySystem inventory;
    [SerializeField] GameObject trashDetailBase;
    int multiplier;

    public void Initialization()
    {
        multiplier = (up == true) ? 1 : -1;
        image = GetComponent<Image>();
        inventory = GameManager.instance.GetInventorySystem;
    }

    public void OnSelect(BaseEventData eventData)
    {
        inventory.SetTrashAmount(multiplier);
        StartCoroutine(DeselectBackToPreviousItem());
    }

    IEnumerator DeselectBackToPreviousItem()
    {
        yield return new WaitForSeconds(0.25f);
        EventSystem.current.SetSelectedGameObject(trashDetailBase);
    }
}
