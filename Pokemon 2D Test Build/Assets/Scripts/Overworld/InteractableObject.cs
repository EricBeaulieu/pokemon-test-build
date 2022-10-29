using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class InteractableObject : MonoBehaviour, IInteractable, ISaveable
{
    [SerializeField] SaveableEntity saveableEntity;
    [Tooltip("The player can only interact with this from the specified direction")]
    [SerializeField] bool specifiedDirectionOnly = false;
    [Tooltip("Only works when Specified Direction Only is true")]
    [SerializeField] FacingDirections interactablesDirection;
    [SerializeField] Dialog dialog;
    [Header("Item Data")]
    [SerializeField] Dialog dialogBeforeItemPickUp;
    [SerializeField] ItemBase itemBase;
    [SerializeField] int count = 1;
    bool itemPickedUp;

    public IEnumerator OnInteract(Vector2 vector2)
    {
        if(specifiedDirectionOnly == true)
        {
            Vector2 curDir = new Vector2(Mathf.Round(transform.position.x - vector2.x), Mathf.Round(transform.position.y - vector2.y));

            if(GlobalTools.CurrentDirectionFacing((Vector2)transform.position,vector2) != interactablesDirection)
            {
                yield break;
            }
        }

        if(itemPickedUp == true || itemBase == null)
        {
            yield return GameManager.instance.GetDialogSystem.ShowDialogBox(dialog);
        }
        else
        {
            DialogManager dialogManager = GameManager.instance.GetDialogSystem;
            yield return dialogManager.ShowDialogBox(dialogBeforeItemPickUp);
            string inGameText;
            if (count > 1)
            {
                inGameText = $"{GameManager.instance.GetPlayerController.TrainerName} found {count} {itemBase.ItemName}'s";
            }
            else
            {
                inGameText = $"{GameManager.instance.GetPlayerController.TrainerName} found {itemBase.ItemName}";
            }
            itemPickedUp = true;
            yield return dialogManager.ShowDialogBox(new Dialog(inGameText));
            GameManager.instance.GetInventorySystem.AddItem(itemBase, count);
            SavingSystem.AddInfoTobeSaved(saveableEntity);
        }
    }

    public object CaptureState(bool playerSave = false)
    {
        return itemPickedUp;
    }

    public void RestoreState(object state)
    {
        itemPickedUp = (bool)state;
    }
}
