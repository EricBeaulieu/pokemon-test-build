using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigma : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Enigma; } }
    public override bool HealsPokemonAfterTakingDamage(BattleUnit holder, bool superEffective)
    {
        if (superEffective == true)
        {
            holder.removeItem = true;
            int hpHealed = Mathf.FloorToInt(holder.pokemon.maxHitPoints / 4);

            if (hpHealed <= 0)
            {
                hpHealed = 1;
            }

            holder.pokemon.UpdateHPRestored(hpHealed);
            holder.pokemon.statusChanges.Enqueue($"{holder.pokemon.currentName} restored HP using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!");

            return true;
        }
        return base.HealsPokemonAfterTakingDamage(holder, superEffective);
    }
}