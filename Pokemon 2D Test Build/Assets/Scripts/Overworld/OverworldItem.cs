using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class OverworldItem : MonoBehaviour,IInteractable,ISaveable
{
    [SerializeField] SaveableEntity saveableEntity;
    [SerializeField] ItemBase itemBase;
    [SerializeField] int count = 1;

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
        gameObject.SetActive(false);
        yield return GameManager.instance.GetDialogSystem.ShowDialogBox(new Dialog(inGameText));
        GameManager.instance.GetInventorySystem.AddItem(itemBase, count);
        SavingSystem.AddInfoTobeSaved(saveableEntity);

        yield break;
    }

    public object CaptureState(bool playerSave = false)
    {
        return new OverworldItemSaveData { itemPickedUp = (gameObject.activeInHierarchy) };
    }

    public void RestoreState(object state)
    {
        var saveData = (OverworldItemSaveData)state;
        gameObject.SetActive(saveData.itemPickedUp);
    }

    [Serializable]
    struct OverworldItemSaveData
    {
        public bool itemPickedUp;
    }
}
