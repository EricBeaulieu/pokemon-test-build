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
        if (holdItem.Id == HoldItemID.ExpShare || holdItem.Id == HoldItemID.MachoBrace || holdItem.Id == HoldItemID.PowerAnklet ||
            holdItem.Id == HoldItemID.PowerBand || holdItem.Id == HoldItemID.PowerBelt || holdItem.Id == HoldItemID.PowerBracer ||
            holdItem.Id == HoldItemID.PowerLens || holdItem.Id == HoldItemID.PowerWeight || holdItem.Id == HoldItemID.LuckyEgg)
        {
            return false;
        }
            return true;
    }
}