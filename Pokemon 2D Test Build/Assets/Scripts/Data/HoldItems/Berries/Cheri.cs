using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheri : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Cheri; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Cheri(); }
    public override bool HealConditionAfterTakingDamage(Pokemon holder)
    {
        if(holder.GetCurrentStatus() == ConditionID.Paralyzed)
        {
            RemoveItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} cured its paralysis using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Paralyzed;
    }
}