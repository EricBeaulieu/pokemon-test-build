using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSystem : CoreSystem
{
    DialogManager dialogSystem;
    InventorySystem inventory;

    [SerializeField] ShopItem shopItemPrefab;
    [SerializeField] RectTransform shopItemHousing;
    [SerializeField] List<ShopItem> allAvailableShopItems;
    static List<ItemBase> currentSoldItems;
    public static ShopItemDetails itemDetails { get; private set; }

    [SerializeField] Text playerMoneyText;
    SelectableBoxUI selectableBox;
    float shopItemHeight;
    const int itemsInViewport = 10;

    [SerializeField] GameObject purchaseDetails;
    int currentPurchasingAmount;
    ItemBase itemBeingPurchased;
    int maxAmount;
    const int MaxAmountPurchable = 99;
    [SerializeField] Text purchaseAmountText;
    const string CANT_AFFORD_MESSAGE = "You can't afford for this";
    bool currentlyPurchasing = false;
    bool inFinalPurchase = false;

    [SerializeField] GameObject upArrow;
    [SerializeField] GameObject downArrow;

    public override void Initialization()
    {
        itemDetails = GetComponentInChildren<ShopItemDetails>();
        dialogSystem = GameManager.instance.GetDialogSystem;
        inventory = GameManager.instance.GetInventorySystem;
        selectableBox = new SelectableBoxUI(allAvailableShopItems[0].gameObject);
        gameObject.SetActive(false);
        shopItemHeight = shopItemPrefab.GetComponent<RectTransform>().rect.height + shopItemHousing.GetComponent<VerticalLayoutGroup>().spacing;
        purchaseDetails.GetComponent<Button>().onClick.AddListener( () => StartCoroutine(ConfirmFinalPurchase()) );
    }

    public override void HandleUpdate()
    {
        if(inFinalPurchase)
        {
            return;
        }

        if(Input.GetButtonDown("Fire2"))
        {
            if(currentlyPurchasing == true)
            {
                ClosePurchaseDetails();
            }
            else
            {
                CloseSystem();
            }
        }

    }

    public override void OpenSystem(bool specifiedBool = false)
    {
        gameObject.SetActive(true);
        dialogSystem.SetCurrentDialogBox(dialogBox);
        dialogSystem.ActivateDialog(false);
        GameManager.SetGameState(GameState.Shop);
        //do something with camera to show the player
        SetupData();
        selectableBox.SelectBox();
        UpdatePlayerMoney();
        currentlyPurchasing = false;
        purchaseDetails.SetActive(false);
    }

    protected override void CloseSystem()
    {
        GameManager.SetGameState(GameState.Overworld);
        dialogSystem.SetCurrentDialogBox();
        gameObject.SetActive(false);
    }

    public static void SetItemsForSale(List<ItemBase> itemsForSale)
    {
        currentSoldItems = itemsForSale;
    }

    void SetupData()
    {
        if(currentSoldItems.Count > allAvailableShopItems.Count)
        {
            int difference = currentSoldItems.Count - allAvailableShopItems.Count;

            for (int i = 0; i < difference; i++)
            {
                ShopItem newShopItem = Instantiate(shopItemPrefab, shopItemHousing);
                allAvailableShopItems.Add(newShopItem);
            }
        }

        for (int i = 0; i < allAvailableShopItems.Count; i++)
        {
            if(i < currentSoldItems.Count)
            {
                allAvailableShopItems[i].SetupData(currentSoldItems[i]);
                allAvailableShopItems[i].gameObject.SetActive(true);
            }
            else
            {
                allAvailableShopItems[i].gameObject.SetActive(false);
            }
        }
    }

    public void HandleScrolling(ShopItem currentlySelected)
    {
        int selectedItem = allAvailableShopItems.IndexOf(currentlySelected);
        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewport/2,0,selectedItem)  * shopItemHeight;
        shopItemHousing.localPosition = new Vector2(shopItemHousing.localPosition.x, scrollPos);

        upArrow.SetActive(selectedItem > itemsInViewport / 2);
        downArrow.SetActive(selectedItem + itemsInViewport / 2 < CurrentActiveItemSlots());
    }

    int CurrentActiveItemSlots()
    {
        int active = 0;
        for (int i = 0; i < allAvailableShopItems.Count; i++)
        {
            if(allAvailableShopItems[i].gameObject.activeInHierarchy == true)
            {
                active++;
            }
        }
        return active;
    }

    public void SelectItemToPurchase(ItemBase itemBase)
    {
        PlayerController player = GameManager.instance.GetPlayerController;
        selectableBox.SetLastSelected(EventSystem.current.currentSelectedGameObject);
        purchaseDetails.SetActive(true);
        if (itemBase.GetItemValue > player.money)
        {
            StartCoroutine(UnableToPurchase());
            return;
        }
        currentlyPurchasing = true;
        dialogSystem.SetCurrentDialogBox(dialogBox);
        dialogSystem.SetDialogText($"{itemBase.ItemName}? Certainly!\n How Many Would You Like?");
        dialogSystem.ActivateDialog(true);
        GameManager.SetGameState(GameState.Shop);
        selectableBox.SelectBox(purchaseDetails);
        itemBeingPurchased = itemBase;
        maxAmount = player.money/itemBase.GetItemValue;
        if(maxAmount > MaxAmountPurchable)
        {
            maxAmount = MaxAmountPurchable;
        }
        currentPurchasingAmount = 1;

        UpdatePurchaseDetails(itemBeingPurchased, currentPurchasingAmount);
    }

    public IEnumerator IncreasePurchaseAmount(int increment)
    {
        yield return new WaitForSeconds(0.01f);
        if (currentPurchasingAmount <= 1 && increment < 0)
        {
            currentPurchasingAmount = maxAmount;
        }
        else if (currentPurchasingAmount >= maxAmount && increment > 0)
        {
            currentPurchasingAmount = 1;
        }
        else
        {
            currentPurchasingAmount += increment;
            currentPurchasingAmount = Mathf.Clamp(currentPurchasingAmount,1, maxAmount);
        }

        UpdatePurchaseDetails(itemBeingPurchased, currentPurchasingAmount);
        selectableBox.SelectBox(purchaseDetails);
    }

    void UpdatePurchaseDetails(ItemBase itemsold,int count)
    {
        purchaseAmountText.text = $"X {count.ToString("00")}     {(itemsold.GetItemValue * count).ToString("C0")}";
    }

    IEnumerator UnableToPurchase()
    {
        selectableBox.Deselect();
        dialogSystem.SetCurrentDialogBox(dialogBox);
        dialogSystem.ActivateDialog(true);
        yield return dialogSystem.TypeDialog(CANT_AFFORD_MESSAGE, true);
        dialogSystem.ActivateDialog(false);
        GameManager.SetGameState(GameState.Shop);
        selectableBox.SelectBox();
    }

    void UpdatePlayerMoney()
    {
        playerMoneyText.text = GameManager.instance.GetPlayerController.money.ToString("C0");
    }

    public IEnumerator ConfirmFinalPurchase()
    {
        inFinalPurchase = true;
        selectableBox.Deselect();
        dialogSystem.SetCurrentDialogBox(dialogBox);
        dialogSystem.ActivateDialog(true);

        int purchaseTotal = itemBeingPurchased.GetItemValue * currentPurchasingAmount;
        yield return dialogSystem.TypeDialog($"{itemBeingPurchased.ItemName}, and you want {currentPurchasingAmount}.\n" +
            $"That will be {purchaseTotal.ToString("C0")}. Okay?");
        bool playerSelection = false;

        yield return dialogSystem.SetChoiceBox(() =>
        {
            playerSelection = true;
        });

        if (playerSelection == true)
        {
            inventory.AddItem(itemBeingPurchased, currentPurchasingAmount);
            GameManager.instance.GetPlayerController.money -= purchaseTotal;
            UpdatePlayerMoney();
            yield return dialogSystem.TypeDialog("Here you are!\nThank you!");
            yield return new WaitForSeconds(0.5f);
            if(itemBeingPurchased is PokeballItem)
            {
                int bonusBall = Mathf.FloorToInt(currentPurchasingAmount / 10);
                if (bonusBall > 0)
                {
                    PokeballItem premierBall = Resources.Load<PokeballItem>("Items/Pokeballs/Premier Ball");

                    if(bonusBall == 1)
                    {
                        yield return dialogSystem.TypeDialog($"You also get a {premierBall.ItemName} as a added bonus");
                    }
                    else
                    {
                        yield return dialogSystem.TypeDialog($"You also recieved {bonusBall} {premierBall.ItemName}'s as a added bonus");
                    }
                    inventory.AddItem(premierBall, bonusBall);
                }
            }

        }

        ClosePurchaseDetails();

        inFinalPurchase = false;
    }

    void ClosePurchaseDetails()
    {
        dialogSystem.ActivateDialog(false);
        GameManager.SetGameState(GameState.Shop);
        selectableBox.SelectBox();
        purchaseDetails.SetActive(false);
        currentlyPurchasing = false;
    }
}
