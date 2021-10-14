using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oran : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Oran; } }
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
    public override bool HealsPokemonAfterTakingDamage(BattleUnit holder, bool superEffective)
    {
        float pokemonHealthPercentage = (float)holder.pokemon.currentHitPoints / (float)holder.pokemon.maxHitPoints;
        if (pokemonHealthPercentage <= StandardBerryUseage(holder.pokemon))
        {
            holder.removeItem = true;

            holder.pokemon.UpdateHPRestored(hpHealed);
            holder.pokemon.statusChanges.Enqueue($"{holder.pokemon.currentName} restored HP using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!");

            return true;
        }
        return base.HealsPokemonAfterTakingDamage(holder,superEffective);
    }
}