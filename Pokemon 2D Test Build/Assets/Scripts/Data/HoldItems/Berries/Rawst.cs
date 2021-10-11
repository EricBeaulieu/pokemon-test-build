using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rawst : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Rawst; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Rawst(); }
    public override bool HealConditionAfterTakingDamage(Pokemon holder)
    {
        if (holder.GetCurrentStatus() == ConditionID.Burn)
        {
            RemoveItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} cured its burn using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Burn;
    }
}