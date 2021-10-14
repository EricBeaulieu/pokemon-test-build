using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pecha : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Pecha; } }
    public override bool HealConditionAfterTakingDamage(BattleUnit holder)
    {
        if (holder.pokemon.GetCurrentStatus() == ConditionID.Poison || holder.pokemon.GetCurrentStatus() == ConditionID.ToxicPoison)
        {
            holder.removeItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} cured its poisoning using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Poison;
    }
}