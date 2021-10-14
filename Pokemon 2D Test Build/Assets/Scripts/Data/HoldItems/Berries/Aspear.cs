using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aspear : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Aspear; } }
    public override bool HealConditionAfterTakingDamage(BattleUnit holder)
    {
        if (holder.pokemon.GetCurrentStatus() == ConditionID.Frozen)
        {
            holder.removeItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} cured its freezing using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Frozen;
    }
}