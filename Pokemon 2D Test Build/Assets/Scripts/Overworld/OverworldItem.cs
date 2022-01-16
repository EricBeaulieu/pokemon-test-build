using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class OverworldItem : MonoBehaviour,IInteractable,ISaveable
{
    [SerializeField] SaveableEntity saveableEntity;
    [SerializeField] ItemBase itemBase;
    public ItemBase CurrentItem { get { return itemBase; } }
    [SerializeField] int count = 1;
    public int CurrentCount { get { return count; } }

    bool itemPickedUp;

    void Start()
    {
        transform.position = GlobalTools.SnapToGrid(transform.position);
    }

    public IEnumerator OnInteract(Vector2 vector2)
    {
        string inGameText;
        if(count >1)
        {
            inGameText = $"You found {count} {itemBase.ItemName}'s";
        }
        else
        {
            inGameText = $"You found {itemBase.ItemName}";
        }
        itemPickedUp = true;
        gameObject.SetActive(false);
        yield return GameManager.instance.GetDialogSystem.ShowDialogBox(new Dialog(inGameText));
        GameManager.instance.GetInventorySystem.AddItem(itemBase, count);
        SavingSystem.AddInfoTobeSaved(saveableEntity);

        yield break;
    }

    public object CaptureState(bool playerSave = false)
    {
        return itemPickedUp;
    }

    public void RestoreState(object state)
    {
        itemPickedUp = (bool)state;
        gameObject.SetActive(!itemPickedUp);
    }
}
