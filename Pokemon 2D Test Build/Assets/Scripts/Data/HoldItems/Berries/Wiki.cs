using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiki : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Wiki; } }
    ConditionID condition = ConditionID.NA;
    Flavor hatedFlavor = Flavor.Dry;
    public override bool HealsPokemonAfterTakingDamage(BattleUnit holder, bool superEffective)
    {
        float pokemonHealthPercentage = (float)holder.pokemon.currentHitPoints / (float)holder.pokemon.maxHitPoints;
        if (pokemonHealthPercentage <= StandardBerryUseage(holder.pokemon))
        {
            holder.removeItem = true;
            int hpHealed = Mathf.FloorToInt(holder.pokemon.maxHitPoints / 3);

            if (hpHealed <= 0)
            {
                hpHealed = 1;
            }

            holder.pokemon.UpdateHPRestored(hpHealed);
            holder.pokemon.statusChanges.Enqueue($"{holder.pokemon.currentName} restored HP using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!");

            if (holder.pokemon.Nature.GetFlavourRating(hatedFlavor) < 0)
            {
                condition = ConditionID.Confused;
            }
            else
            {
                condition = ConditionID.NA;
            }

            return true;
        }
        return base.HealsPokemonAfterTakingDamage(holder,superEffective);
    }
    public override ConditionID AdditionalEffects()
    {
        return condition;
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        if (condition == ConditionID.NA)
        {
            return "";
        }
        return $"For {holder.pokemon.currentName}, the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry was too {hatedFlavor}!";
    }
}