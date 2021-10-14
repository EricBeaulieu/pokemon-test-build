using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persim : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Persim; } }
    public override bool HealConditionAfterTakingDamage(BattleUnit holder)
    {
        if (holder.pokemon.HasCurrentVolatileStatus(ConditionID.Confused) == true)
        {
            holder.removeItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} cured its confusion using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Confused;
    }
}