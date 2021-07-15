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

    public abstract bool UseItem(Pokemon pokemon);

    public abstract bool UseItemOption();
    public virtual bool GiveItemOption()
    {
        if (BattleSystem.inBattle == true)
        {
            return false;
        }
        return true;
    }
    public virtual bool TrashItemOption()//Key item will always be false when implimented
    {
        if (BattleSystem.inBattle == true)
        {
            return false;
        }
        return true;
    }

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
