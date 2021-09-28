using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour, ISaveable
{
    [SerializeField] SaveableEntity saveableEntity;
    [SerializeField] ItemBase item;
    [SerializeField] int itemCount = 1;

    [SerializeField] Dialog dialogBeforeItemGiven;
    [SerializeField] Dialog dialogAfterItemGiven;
    bool itemGiven = false;

    public IEnumerator GiveItem(PlayerController player)
    {
        DialogManager dialogManager = GameManager.instance.GetDialogSystem;

        yield return dialogManager.ShowDialogBox(dialogBeforeItemGiven);

        if(itemCount <=0)
        {
            Debug.LogWarning("the item count on this item giver component is less then 1", gameObject);
            itemCount = 1;
        }

        GameManager.instance.GetInventorySystem.AddItem(item, itemCount);
        itemGiven = true;
        SavingSystem.AddInfoTobeSaved(saveableEntity);

        dialogManager.ActivateDialog(true);
        if (itemCount > 1)
        {
            yield return dialogManager.TypeDialog($"{player.TrainerName} recieved {itemCount} {item.ItemName}'s",true);
        }
        else
        {
            yield return dialogManager.TypeDialog($"{player.TrainerName} recieved {item.ItemName}",true);
        }
        dialogManager.ActivateDialog(false);

        if (dialogAfterItemGiven.Lines.Count > 0)
        {
            yield return dialogManager.ShowDialogBox(dialogAfterItemGiven);
        }
    }

    public bool ItemCanBeGiven()
    {
        return item != null && !itemGiven;
    }

    public object CaptureState(bool playerSave = false)
    {
        return itemGiven;
    }

    public void RestoreState(object state)
    {
        itemGiven = (bool)state;
    }
}
