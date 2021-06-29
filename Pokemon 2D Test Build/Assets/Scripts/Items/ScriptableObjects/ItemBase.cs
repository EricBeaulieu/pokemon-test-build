using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType { Basic, Medicine, Pokeball, TMHM,Berry,Battle, KeyItem}

public abstract class ItemBase : ScriptableObject
{
    protected itemType itemType;

    [SerializeField] string itemName;
    [TextArea]
    [SerializeField] string itemDescription;
    [SerializeField] Sprite itemSprite;

    public abstract void UseItem();

    public itemType GetItemType
    {
        get { return itemType; }
    }

    public string ItemName
    {
        get { return itemName; }
    }

    public string ItemDescription
    {
        get { return itemDescription; }
    }

    public Sprite ItemSprite
    {
        get { return itemSprite; }
    }

    public virtual HoldItemBase HoldItemAffects()
    {
        return HoldItemDB.GetHoldItem(HoldItemID.NA);
    }
}
