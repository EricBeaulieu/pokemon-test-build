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

    /// <summary>
    /// Checks to see if the item is compatable with the pokemon
    /// </summary>
    /// <param name="pokemon">Pokemon the item is being used on</param>
    /// <returns>uses the item</returns>
    public abstract bool UseItem(Pokemon pokemon);

    /// <summary>
    /// Checks if this item type can be used in the current situation, if overworld or battle
    /// </summary>
    /// <returns></returns>
    public abstract bool UseItemOption();
    public virtual bool GiveItemOption()
    {
        return !BattleSystem.inBattle;
    }
    public virtual bool TrashItemOption()//Key item will always be false when implimented
    {
        return !BattleSystem.inBattle;
    }

    public itemType GetItemType
    {
        get { return itemType; }
    }

    public virtual string ItemName
    {
        get { return itemName; }
    }

    public virtual string ItemDescription
    {
        get { return itemDescription; }
    }

    public virtual Sprite ItemSprite
    {
        get { return itemSprite; }
    }

    public virtual HoldItemBase HoldItemAffects()
    {
        return HoldItemDB.GetHoldItem(HoldItemID.NA);
    }

    /// <summary>
    /// If false show the Usable UI
    /// </summary>
    /// <returns></returns>
    public virtual bool ShowStandardUI()
    {
        return true;
    }

    public virtual bool AbleOrUnableToUseOnPokemon(PokemonBase pokemon)
    {
        return false;
    }
}
