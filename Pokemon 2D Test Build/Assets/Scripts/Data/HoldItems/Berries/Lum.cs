using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lum : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Lum; } }
    public override bool HealConditionAfterTakingDamage(BattleUnit holder)
    {
        if (holder.pokemon.volatileStatus.Count > 0)
        {
            holder.removeItem = true;
            return true;
        }
        return base.HealConditionAfterTakingDamage(holder);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} cured its ailment using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
    public override ConditionID AdditionalEffects()
    {
        return ConditionID.Confused;
    }
}