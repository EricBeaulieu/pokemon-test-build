using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    InventorySystem inventory;

    Item currentItem;
    [SerializeField] Image standardBackground;
    [SerializeField] GameObject background;
    [SerializeField] GameObject selector;
    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;
    [SerializeField] Text itemCount;
    DialogManager messageBox;
    [SerializeField] Button button;

    void Awake()
    {
        inventory = GameManager.instance.GetInventorySystem;
        messageBox = GameManager.instance.GetDialogSystem;
    }

    public void SetData(Item item,Color missingColor)
    {
        currentItem = item;
        button.onClick.RemoveAllListeners();
        EnableSelector(false);

        if (item == null)
        {
            standardBackground.color = missingColor;
            background.SetActive(false);
            itemSprite.color = Color.clear;
            itemName.text = "";
            itemCount.text = "";
            return;
        }
        standardBackground.color = Color.white;
        background.SetActive(true);
        itemSprite.color = Color.white;
        itemSprite.sprite = item.ItemBase.ItemSprite;
        itemName.text = $"{item.ItemBase.ItemName}";
        itemCount.text = $"x {item.Count}";
        button.onClick.AddListener(() => { StartCoroutine(OnClick()); });
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetMessageText(currentItem);
        inventory.SetLastItemButton(this.gameObject);
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

    void SetMessageText(Item item)
    {
        if(item == null)
        {
            messageBox.SetDialogText("");
        }
        else
        {
            messageBox.SetDialogText(item.ItemBase.ItemDescription);
        }
    }

    IEnumerator OnClick()
    {
        inventory.SetLastItemButton(this.gameObject);
        EventSystem.current.SetSelectedGameObject(null);

        float elapsedTime = 0;
        float stepDuration = 0.2f;
        int totalSteps = 4;

        for (int i = 0; i < totalSteps; i++)
        {
            while (elapsedTime < stepDuration)
            {
                EnableSelector(i % 2 == 1);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0;
        }

        inventory.CurrentItemSelected(currentItem);
    }

    public Button GetButton
    {
        get { return button; }
    }
}
