using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Klutz : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Klutz; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Klutz(); }
    public override string Description()
    {
        return "";
    }
    public override bool CantUseAnyHeldItems(HoldItemBase holdItem)
    {
        if (holdItem.HoldItemId == HoldItemID.ExpShare || holdItem.HoldItemId == HoldItemID.MachoBrace || holdItem.HoldItemId == HoldItemID.PowerAnklet ||
            holdItem.HoldItemId == HoldItemID.PowerBand || holdItem.HoldItemId == HoldItemID.PowerBelt || holdItem.HoldItemId == HoldItemID.PowerBracer ||
            holdItem.HoldItemId == HoldItemID.PowerLens || holdItem.HoldItemId == HoldItemID.PowerWeight || holdItem.HoldItemId == HoldItemID.LuckyEgg)
        {
            return false;
        }
            return true;
    }
}