using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aguav : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Aguav; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Aguav(); }
    ConditionID condition = ConditionID.NA;
    Flavor hatedFlavor = Flavor.Sweet;
    public override bool HealsPokemonAfterAttack(Pokemon pokemon)
    {
        if((pokemon.currentHitPoints/pokemon.maxHitPoints) <= StandardBerryUseage(pokemon))
        {
            RemoveItem = true;
            int hpHealed = Mathf.FloorToInt(pokemon.maxHitPoints / 3);

            if (hpHealed <= 0)
            {
                hpHealed = 1;
            }

            pokemon.UpdateHPRestored(hpHealed);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} restored HP using {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!");

            if(pokemon.nature.GetFlavourRating(hatedFlavor) < 0)
            {
                condition = ConditionID.Confused;
            }

            return true;
        }
        return base.HealsPokemonAfterAttack(pokemon);
    }
    public override ConditionID AdditionalEffects()
    {
        return condition;
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        if(condition == ConditionID.NA)
        {
            return "";
        }
        return $"For {holder.currentName}, the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry was too {hatedFlavor}!";
    }
}
