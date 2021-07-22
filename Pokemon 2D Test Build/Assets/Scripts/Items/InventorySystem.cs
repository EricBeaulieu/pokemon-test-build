using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    BattleSystem battleSystemReference;
    PartySystem partySystemReference;
    [SerializeField] List<Item> currentInventory;

    [SerializeField] List<ItemMenuButtonUI> menuButtons;
    [SerializeField] List<ItemButtonUI> itemButtons;
    [SerializeField] List<ArrowButtonUI> arrowButtons;
    [SerializeField] Button cancelButton;
    [SerializeField] InventoryColorSystem colorSystem;
    [SerializeField] Text inventoryIndex;
    [SerializeField] ItemColorScheme[] colorScheme;
    [SerializeField] InventoryDialogBox dialogBox;

    [SerializeField] GameObject trashDetails;
    [SerializeField] Text trashDetailsCountText;
    [SerializeField] List<TrashArrowButtons> trashArrowButtons;
    static int itemTrashAmount;

    [SerializeField] GameObject currentItemOptionsMenu;
    [SerializeField] ItemOptionDetailsUI itemDetails;
    [SerializeField] ItemOptionButtonUI useOption;
    [SerializeField] ItemOptionButtonUI giveOption;
    [SerializeField] ItemOptionButtonUI trashOption;
    static Item specifiedItem;

    List<Item> currentItemsDisplayed = new List<Item>();
    itemType currentlySelected;
    int currentIndex;
    int pages;

    GameObject lastSelected;

    const int MAX_ITEMS_DISPLAY = 6;

    public void Initialization()
    {
        gameObject.SetActive(false);
        battleSystemReference = GameManager.instance.GetBattleSystem;
        partySystemReference = GameManager.instance.GetPartySystem;
        CheckAndCorrectInventoryUponStart();
        SetItemButtonFunctionality();
        SetMenuButtonFunctionality();
        SetArrowButtonFunctionaility();
        SetTrashArrowButtonFunctionaility();
    }

    public void HandleUpdate()
    {
        //If B button is pressed go back a menu
        if (Input.GetButtonDown("Fire2"))
        {
            CloseInventorySystem();
        }
    }

    public void OpenInventorySystem()
    {
        GameManager.SetGameState(GameState.Inventory);
        gameObject.SetActive(true);
        SetData(currentlySelected);
        SpecificItemOptionDisplay(false);
        SelectBox();
        SetUpCancelButton();
    }

    void CloseInventorySystem()
    {
        if (BattleSystem.inBattle == true)
        {
            GameManager.SetGameState(GameState.Battle);
        }
        else
        {
            GameManager.SetGameState(GameState.Overworld);
        }
        gameObject.SetActive(false);
    }

    void SelectBox(GameObject gameObject = null)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if(gameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(menuButtons[(int)currentlySelected].gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        
    }

    void SetUpCancelButton()
    {
        SetUpCancelButtonFuntionality(true);
        cancelButton.GetComponent<ItemCancelUI>().OnHandleStart();
    }

    public void SetData(itemType itemType = itemType.Basic)
    {
        menuButtons.Find(x => x.ItemMenuType == currentlySelected).ActiveMenu(false);
        currentlySelected = itemType;
        ItemMenuButtonUI currentMenu = menuButtons.Find(x => x.ItemMenuType == currentlySelected);
        currentMenu.ActiveMenu(true);
        colorSystem.SetNewScheme(ColorScheme(currentlySelected));

        currentIndex = 0;
        currentItemsDisplayed.Clear();
        currentItemsDisplayed.AddRange(currentInventory.Where(x => x.ItemBase.GetItemType == itemType));
        pages = (currentItemsDisplayed.Count - 1) / 6;
        
        DisplayCurrentItems(currentlySelected,currentIndex);
        SetUpPagesDynamically(pages);
    }

    void DisplayCurrentItems(itemType menu, int index)
    {
        int offset = index * MAX_ITEMS_DISPLAY;
        for (int i = 0; i < MAX_ITEMS_DISPLAY; i++)
        {
            if (i + offset > currentItemsDisplayed.Count - 1)
            {
                itemButtons[i].SetData(null, ColorScheme(menu).GetMissingItemFadeColor);
            }
            else
            {
                itemButtons[i].SetData(currentItemsDisplayed[i + offset], ColorScheme(menu).GetMissingItemFadeColor);
            }
        }

        inventoryIndex.text = $"{currentIndex + 1} / {pages + 1}";
    }

    void CheckAndCorrectInventoryUponStart()
    {
        if (currentInventory.Count >= 1)
        {
            for (int i = currentInventory.Count - 1; i >= 0; i--)
            {
                if(currentInventory[i] == null || currentInventory[i].ItemBase == null || currentInventory[i].Count <= 0)
                {
                    currentInventory.RemoveAt(i);
                    continue;
                }
                for (int j = currentInventory.Count - 1; j >= 0; j--)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (currentInventory[i].ItemBase == currentInventory[j].ItemBase)
                    {
                        currentInventory[i].Count += currentInventory[j].Count;
                        currentInventory.RemoveAt(j);
                    }
                }
            }
        }

    }

    void SetItemButtonFunctionality()
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].Initialization(this);
        }
    }

    void SetMenuButtonFunctionality()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            int k = i;
            menuButtons[i].GetButton.onClick.AddListener(() => SetData(menuButtons[k].ItemMenuType));
        }
    }

    void SetArrowButtonFunctionaility()
    {
        for (int i = 0; i < arrowButtons.Count; i++)
        {
            arrowButtons[i].Initialization(this);
        }
    }

    void SetTrashArrowButtonFunctionaility()
    {
        for (int i = 0; i < trashArrowButtons.Count; i++)
        {
            trashArrowButtons[i].Initialization();
        }
    }

    public GameObject ReturnToLastButtonPressed()
    {
        return lastSelected.gameObject;
    }

    public void SetLastItemButton(GameObject gameObject)
    {
        lastSelected = gameObject;
    }

    public void LoadNextPage(bool right)
    {
        if (right)
        {
            currentIndex++;
            if (currentIndex > pages)
            {
                currentIndex = 0;
            }
        }
        else
        {
            currentIndex--;
            if(currentIndex < 0)
            {
                currentIndex = pages;
            }
        }
        DisplayCurrentItems(currentlySelected, currentIndex);
    }

    public ItemColorScheme ColorScheme(itemType itemType)
    {
        return colorScheme[(int)itemType];
    }

    void SetUpPagesDynamically(int numberOfPages)
    {
        if(numberOfPages >= 1)
        {
            for (int i = 0; i < arrowButtons.Count; i++)
            {
                arrowButtons[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < MAX_ITEMS_DISPLAY; i++)
            {
                var navigation = itemButtons[i].GetButton.navigation;
                if(i % 2 == 0)
                {
                    navigation.selectOnLeft = arrowButtons[i % 2].GetComponent<Button>();
                }
                else
                {
                    navigation.selectOnRight = arrowButtons[i % 2].GetComponent<Button>();
                }
                itemButtons[i].GetButton.navigation = navigation;
            }
            inventoryIndex.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < arrowButtons.Count; i++)
            {
                arrowButtons[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < MAX_ITEMS_DISPLAY; i++)
            {
                var navigation = itemButtons[i].GetButton.navigation;
                if (i % 2 == 0)
                {
                    navigation.selectOnLeft = null;
                }
                else
                {
                    navigation.selectOnRight = null;
                }
                itemButtons[i].GetButton.navigation = navigation;
            }
            inventoryIndex.gameObject.SetActive(false);
        }
    }

    public void CurrentItemSelected(Item item)
    {
        Color background = ColorScheme(item.ItemBase.GetItemType).GetMissingItemFadeColor;

        itemDetails.SetData(item);
        useOption.SetData(background, item.ItemBase.UseItemOption());
        giveOption.SetData(background, item.ItemBase.GiveItemOption());
        trashOption.SetData(background, item.ItemBase.TrashItemOption());
        specifiedItem = item;

        if(useOption.CurrentlyActive == true)
        {
            useOption.GetButton.onClick.AddListener(delegate { UseOptionSelected(item); });
        }

        if(giveOption.CurrentlyActive == true)
        {
            giveOption.GetButton.onClick.AddListener(delegate { GiveOptionSelected(item); });
        }

        if(trashOption.CurrentlyActive == true)
        {
            trashOption.GetButton.onClick.AddListener(delegate { TrashOptionSelected(item); });
        }

        SpecificItemOptionDisplay(true);
        SelectBox(useOption.gameObject);
    }

    void UseOptionSelected(Item item)
    {
        EventSystem.current.SetSelectedGameObject(null);
        switch (item.ItemBase.GetItemType)
        {
            case itemType.Basic:
                break;
            case itemType.Medicine:
                CloseInventorySystem();
                partySystemReference.OpenPartySystemDueToInventoryItem(item, true);
                break;
            case itemType.Pokeball:
                battleSystemReference.UsePokeballFromInventory((PokeballItem)item.ItemBase);
                RemoveItem(item);
                CloseInventorySystem();
                break;
            case itemType.TMHM:
                break;
            case itemType.Berry:
                break;
            case itemType.Battle:
                break;
            case itemType.KeyItem:
                break;
            default:
                break;
        }
    }

    void GiveOptionSelected(Item item)
    {
        EventSystem.current.SetSelectedGameObject(null);
        CloseInventorySystem();
        partySystemReference.OpenPartySystemDueToInventoryItem(item, false);
    }

    void TrashOptionSelected(Item item)
    {
        specifiedItem = item;
        itemTrashAmount = specifiedItem.Count;
        trashDetails.gameObject.SetActive(true);
        if(specifiedItem.Count > 1)
        {
            EventSystem.current.SetSelectedGameObject(trashDetails);
            Button trashDetailsButton = trashDetails.GetComponent<Button>();
            trashDetailsButton.onClick.RemoveAllListeners();
            trashDetailsButton.onClick.AddListener(() => { StartCoroutine(TrashAmountSetandSelected()); });
        }
        else
        {
            StartCoroutine(TrashAmountSetandSelected());
        }
        EnableOptionsButtons(false);
        SetTrashDetailsCount(itemTrashAmount);
    }

    void SpecificItemOptionDisplay(bool isOn)
    {
        SwitchAllItemButtonsToActive(!isOn);
        SwitchAllMenuButtonsToActive(!isOn);
        if (isOn == true)
        {
            SwitchArrowButtonsToActive(false);
            inventoryIndex.gameObject.SetActive(false);
        }
        currentItemOptionsMenu.SetActive(isOn);
        SetUpCancelButtonFuntionality(!isOn);
    }

    void SwitchAllItemButtonsToActive(bool isOn)
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].gameObject.SetActive(isOn);
        }
    }

    void SwitchAllMenuButtonsToActive(bool isOn)
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].gameObject.SetActive(isOn);
        }
    }

    void SwitchArrowButtonsToActive(bool isOn)
    {
        for (int i = 0; i < arrowButtons.Count; i++)
        {
            arrowButtons[i].gameObject.SetActive(isOn);
        }
    }

    void SetUpCancelButtonFuntionality(bool standardMenu)
    {
        cancelButton.onClick.RemoveAllListeners();

        if (standardMenu == true)
        {
            cancelButton.onClick.AddListener(() => { CloseInventorySystem(); });

            var navigation = cancelButton.navigation;
            navigation.selectOnUp = itemButtons[5].GetButton;
            navigation.selectOnDown = menuButtons[3].GetButton;
            navigation.selectOnLeft = null;
            navigation.selectOnRight = null;
            cancelButton.navigation = navigation;

            if(BattleSystem.inBattle == true)
            {
                cancelButton.onClick.AddListener(() => 
                {
                    battleSystemReference.ReturnFromPokemonAlternateSystem();
                });
            }
        }
        else
        {
            cancelButton.onClick.AddListener(() => 
            {
                SpecificItemOptionDisplay(false);

                if(pages > 0)
                {
                    SwitchArrowButtonsToActive(true);
                    inventoryIndex.gameObject.SetActive(true);
                }

                EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
            });

            var navigation = cancelButton.navigation;
            navigation.selectOnUp = trashOption.GetButton;
            navigation.selectOnDown = trashOption.GetButton;
            navigation.selectOnLeft = null;
            navigation.selectOnRight = null;
            cancelButton.navigation = navigation;
        }
    }

    public void ReturnFromPartySystemAfterItemUsage(bool usingItem)
    {
        GameManager.SetGameState(GameState.Inventory);
        SetData(specifiedItem.ItemBase.GetItemType);
        if (specifiedItem.Count > 0)
        {
            gameObject.SetActive(true);
            if(usingItem ==true)
            {
                SelectBox(useOption.gameObject);
            }
            else
            {
                SelectBox(giveOption.gameObject);
            }
            SetUpCancelButtonFuntionality(false);
            itemDetails.SetData(specifiedItem);
        }
        else
        {
            gameObject.SetActive(true);
            specifiedItem = null;
            SpecificItemOptionDisplay(false);
            SelectBox(lastSelected);
            SetUpCancelButtonFuntionality(true);
        }
    }
    
    public void RemoveItem(Item item,int count = 1)
    {
        item.Count -= count;
        if (item.Count <= 0)
        {
            currentInventory.Remove(item);
        }
    }

    public void RemoveItem(ItemBase item,int count = 1)
    {
        RemoveItem(currentInventory.Find(x => x.ItemBase == item),count);
    }

    public void AddItem(ItemBase item, int count = 1)
    {
        if(currentInventory.Exists(x => x.ItemBase == item) == true)
        {
            currentInventory.Find(x => x.ItemBase == item).Count += count;
        }
        else
        {
            Item newItem = new Item() { ItemBase = item, Count = count };
            currentInventory.Add(newItem);
        }
    }

    public void SetTrashAmount(int addition)
    {
        itemTrashAmount += addition;
        if(itemTrashAmount <= 0)
        {
            itemTrashAmount = specifiedItem.Count;
        }
        else if( itemTrashAmount > specifiedItem.Count)
        {
            itemTrashAmount = 1;
        }
        SetTrashDetailsCount(itemTrashAmount);
    }

    public void SetTrashDetailsCount(int count)
    {
        trashDetailsCountText.text = $"X {count.ToString("000")}";
    }

    void EnableOptionsButtons(bool enable)
    {
        useOption.gameObject.SetActive(enable);
        giveOption.gameObject.SetActive(enable);
        trashOption.gameObject.SetActive(enable);
    }

    IEnumerator TrashAmountSetandSelected()
    {
        dialogBox.SetDialogText($"Is it Ok to throw away {itemTrashAmount} {specifiedItem.ItemBase.ItemName}");
        EventSystem.current.SetSelectedGameObject(null);

        yield return new WaitForSeconds(1f);
        yield return dialogBox.SetChoiceBox(
            () =>//Yes 
            {
                RemoveItem(specifiedItem, itemTrashAmount);
                ReturnToMenuAfterTrashDetails();
            },
            () =>//No
            {
                ReturnToMenuAfterTrashDetails();
            });
    }

    void ReturnToMenuAfterTrashDetails()
    {
        EnableOptionsButtons(true);
        SetData(specifiedItem.ItemBase.GetItemType);
        trashDetails.SetActive(false);
        specifiedItem = null;
        SpecificItemOptionDisplay(false);
        SelectBox(lastSelected);
        SetUpCancelButtonFuntionality(true);
    }
}
