using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pecha : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Pecha; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Pecha(); }
    public override bool HealConditionAfterTakingDamage(Pokemon holder)
    {
        if (holder.GetCurrentStatus() == ConditionID.Poison || holder.GetCurrentStatus() == ConditionID.ToxicPoison)
        {
            RemoveItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} cured its poisoning using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Poison;
    }
}