using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iapapa : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Iapapa; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Iapapa(); }
    ConditionID condition = ConditionID.NA;
    Flavor hatedFlavor = Flavor.Sour;
    public override bool HealsPokemonAfterTakingDamage(Pokemon pokemon)
    {
        float pokemonHealthPercentage = (float)pokemon.currentHitPoints / (float)pokemon.maxHitPoints;
        if (pokemonHealthPercentage <= StandardBerryUseage(pokemon))
        {
            RemoveItem = true;
            int hpHealed = Mathf.FloorToInt(pokemon.maxHitPoints / 3);

            if (hpHealed <= 0)
            {
                hpHealed = 1;
            }

            pokemon.UpdateHPRestored(hpHealed);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} restored HP using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!");

            if (pokemon.nature.GetFlavourRating(hatedFlavor) < 0)
            {
                condition = ConditionID.Confused;
            }

            return true;
        }
        return base.HealsPokemonAfterTakingDamage(pokemon);
    }
    public override ConditionID AdditionalEffects()
    {
        return condition;
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        if (condition == ConditionID.NA)
        {
            return "";
        }
        return $"For {holder.currentName}, the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry was too {hatedFlavor}!";
    }
}