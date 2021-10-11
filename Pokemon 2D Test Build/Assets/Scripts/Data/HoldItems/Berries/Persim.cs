using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persim : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Persim; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Persim(); }
    public override bool HealConditionAfterTakingDamage(Pokemon holder)
    {
        if (holder.HasCurrentVolatileStatus(ConditionID.Confused) == true)
        {
            RemoveItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} cured its confusion using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Confused;
    }
}