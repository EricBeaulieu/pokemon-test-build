using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesto : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Chesto; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Chesto(); }
    public override bool HealConditionAfterTakingDamage(Pokemon holder)
    {
        if (holder.GetCurrentStatus() == ConditionID.Sleep)
        {
            RemoveItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} cured its sleep using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Sleep;
    }
}