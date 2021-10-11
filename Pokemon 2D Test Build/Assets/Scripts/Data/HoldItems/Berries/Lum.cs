using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lum : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Lum; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Lum(); }
    public override bool HealConditionAfterTakingDamage(Pokemon holder)
    {
        if (holder.volatileStatus.Count > 0)
        {
            RemoveItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} cured its ailment using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Confused;
    }
}