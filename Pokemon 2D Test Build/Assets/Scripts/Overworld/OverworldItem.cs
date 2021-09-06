using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OverworldItem : MonoBehaviour,IInteractable,ISaveable
{
    [SerializeField] ItemBase itemBase;
    [SerializeField] int count = 1;

    void Start()
    {
        //TO DO save item and delete if not needed
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

        yield break;
    }

    public object CaptureState()
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
