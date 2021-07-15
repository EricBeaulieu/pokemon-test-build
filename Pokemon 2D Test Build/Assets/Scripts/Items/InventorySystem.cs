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
    public Action onCloseInventory;
    [SerializeField] List<Item> currentInventory;

    [SerializeField] List<ItemMenuButtonUI> menuButtons;
    [SerializeField] List<ItemButtonUI> itemButtons;
    [SerializeField] List<ArrowButtonUI> arrowButtons;
    [SerializeField] Button cancelButton;
    [SerializeField] InventoryColorSystem colorSystem;
    [SerializeField] Text inventoryIndex;
    [SerializeField] ItemColorScheme[] colorScheme;

    [SerializeField] GameObject currentItemOptionsMenu;
    [SerializeField] ItemOptionDetailsUI itemDetails;
    [SerializeField] ItemOptionButtonUI useOption;
    [SerializeField] ItemOptionButtonUI giveOption;
    [SerializeField] ItemOptionButtonUI trashOption;

    List<Item> currentItemsDisplayed = new List<Item>();
    itemType currentlySelected;
    int currentIndex;
    int pages;

    GameObject lastSelected;

    const int MAX_ITEMS_DISPLAY = 6;

    GameObject _lastSelected;

    const int MESSAGEBOX_STANDARD_SIZE = 650;
    const int MESSAGEBOX_SELECTED_SIZE = 515;

    public void Initialization()
    {
        battleSystemReference = GameManager.instance.GetBattleSystem;
        partySystemReference = GameManager.instance.GetPartySystem;
        CheckAndCorrectInventoryUponStart();
        SetItemButtonFunctionality();
        SetMenuButtonFunctionality();
        SetArrowButtonFunctionaility();
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
        gameObject.SetActive(true);
        SetData();
        _lastSelected = null;
        SpecificItemOptionDisplay(false);
        SelectBox();
        SetUpCancelButton();
    }

    void CloseInventorySystem()
    {
        EventSystem.current.SetSelectedGameObject(null);
        onCloseInventory();
    }

    void SelectBox(GameObject gameObject = null)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if(gameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
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
        pages = currentItemsDisplayed.Count / 6;
        
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

        useOption.GetButton.onClick.AddListener(delegate { UseOptionSelected(item); });
        giveOption.GetButton.onClick.AddListener(delegate { GiveOptionSelected(item); });
        trashOption.GetButton.onClick.AddListener(delegate { TrashOptionSelected(item); });

        SpecificItemOptionDisplay(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(useOption.gameObject);
    }

    void UseOptionSelected(Item item)
    {
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
        Debug.Log($"give button clicked");
    }

    void TrashOptionSelected(Item item)
    {
        Debug.Log($"Trash button clicked");
    }

    void SpecificItemOptionDisplay(bool isOn)
    {
        SwitchAllItemButtonsToActive(!isOn);
        SwitchAllMenuButtonsToActive(!isOn);
        SwitchArrowButtonsToActive(!isOn);
        inventoryIndex.gameObject.SetActive(!isOn);
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

    public void ReturnFromPartySystemAfterItemUsage()
    {

        //if item is above 0
        SpecificItemOptionDisplay(false);
        SelectBox();
        SetUpCancelButton();
    }
    
}
