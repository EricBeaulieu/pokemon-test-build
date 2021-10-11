using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oran : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Oran; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Oran(); }
    const int hpHealed = 10;
    public override bool UseInInventory()
    {
        return true;
    }
    public override bool UsedInInventoryEffect(Pokemon pokemon)
    {
        if(pokemon.currentHitPoints > 0 && pokemon.currentHitPoints < pokemon.maxHitPoints)
        {
            pokemon.UpdateHPRestored(hpHealed);
            return true;
        }
        return false;
    }
    public override bool HealsPokemonAfterTakingDamage(Pokemon pokemon)
    {
        float pokemonHealthPercentage = (float)pokemon.currentHitPoints / (float)pokemon.maxHitPoints;
        if (pokemonHealthPercentage <= StandardBerryUseage(pokemon))
        {
            RemoveItem = true;

            pokemon.UpdateHPRestored(hpHealed);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} restored HP using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!");

            return true;
        }
        return base.HealsPokemonAfterTakingDamage(pokemon);
    }
}