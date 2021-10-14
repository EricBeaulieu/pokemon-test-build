using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sitrus : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Sitrus; } }
    int hpHealed;
    public override bool UseInInventory()
    {
        return true;
    }
    public override bool UsedInInventoryEffect(Pokemon pokemon)
    {
        if (pokemon.currentHitPoints > 0 && pokemon.currentHitPoints < pokemon.maxHitPoints)
        {
            hpHealed = Mathf.FloorToInt(pokemon.maxHitPoints / 3);

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
            hpHealed = Mathf.FloorToInt(holder.pokemon.maxHitPoints/3);

            holder.pokemon.UpdateHPRestored(hpHealed);
            holder.pokemon.statusChanges.Enqueue($"{holder.pokemon.currentName} restored HP using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!");

            return true;
        }
        return base.HealsPokemonAfterTakingDamage(holder,superEffective);
    }
}