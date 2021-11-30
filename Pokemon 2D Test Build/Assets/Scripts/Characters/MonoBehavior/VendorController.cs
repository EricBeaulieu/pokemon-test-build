using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorController : EntityAI, IInteractable
{
    [Header("Vendor Controller")]
    [SerializeField]
    Dialog greetingDialog = new Dialog(new List<string>()
    {
        "Hi there!\nWelcome to our Pokemart",
        "Would you like to browse our selection?"
    });
    [SerializeField] Dialog farewellDialog = new Dialog("Please come again!");
    DialogManager dialogManager;

    [SerializeField] List<ConditionedSellableItem> standardItems = new List<ConditionedSellableItem>();

    void Awake()
    {
        base.Initialization();
        dialogManager = GameManager.instance.GetDialogSystem;
    }

    public override void HandleUpdate()
    {

    }

    public IEnumerator OnInteract(Vector2 initiator)
    {
        GameManager.SetGameState(GameState.Dialog);
        FaceTowardsDirection(initiator);
        dialogManager.ActivateDialog(true);
        for (int i = 0; i < greetingDialog.Lines.Count; i++)
        {
            yield return dialogManager.TypeDialog(greetingDialog.Lines[i]);
        }

        bool playerSelection = false;
        yield return GameManager.instance.GetDialogSystem.SetChoiceBox(() =>
        {
            playerSelection = true;
        }, () =>
        {
            playerSelection = false;
        });

        if (playerSelection == true)
        {
            dialogManager.ActivateDialog(false);
            ShopSystem.SetItemsForSale(ItemsForSale());
            GameManager.instance.GetShopSystem.OpenSystem();
        }
        else
        {
            for (int i = 0; i < farewellDialog.Lines.Count; i++)
            {
                yield return dialogManager.TypeDialog(farewellDialog.Lines[i]);
            }

            dialogManager.ActivateDialog(false);
            GameManager.SetGameState(GameState.Overworld);
        }


    }

    List<ItemBase> ItemsForSale()
    {
        List<ItemBase> sellableItems = new List<ItemBase>();
        for (int i = 0; i < standardItems.Count; i++)
        {
            if(standardItems[i].BadgesRequired <= 8)//players badge amount here
            {
                sellableItems.Add(standardItems[i].ItemBase);
            }
        }

        return sellableItems;
    }
}
