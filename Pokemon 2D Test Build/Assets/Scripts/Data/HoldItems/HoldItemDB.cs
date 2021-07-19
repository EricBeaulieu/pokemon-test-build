using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoldItemID
{
    NA,
    AbsorbBulb, AdamantOrb ,AirBalloon, AmuletCoin, AssaultVest,
    BigRoot, BindingBand, BlackBelt, BlackGlasses, BlackSludge,
    BlunderPolicy, BrightPowder, BugGem, CellBattery, Charcoal,
    ChoiceBand, ChoiceScarf, ChoiceSpecs, CleanseTag, DampRock
}

public class HoldItemDB
{
    public static void Initialization(List<HoldItemBase> holdItems)
    {
        foreach (HoldItemBase item in holdItems)
        {
            HoldItemDex.Add(item.Id, item);
        }
    }

    static Dictionary<HoldItemID, HoldItemBase> HoldItemDex = new Dictionary<HoldItemID, HoldItemBase>();

    public static HoldItemBase GetHoldItem(HoldItemID iD)
    {
        return HoldItemDex[iD].ReturnDerivedClassAsNew();
    }
}
