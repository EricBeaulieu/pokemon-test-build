using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteHerb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.WhiteHerb; } }
    public override bool RestoresAllLoweredStatsToNormalAfterAttackFinished(BattleUnit holder)
    {
        foreach (var stat in holder.pokemon.statBoosts)
        {
            if (holder.pokemon.statBoosts[stat.Key] < 0)
            {
                holder.pokemon.RestoreAllLoweredStatBoosts();
                holder.removeItem = true;
                return true;
            }
        }
        return false;
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} returned its stats to normal using {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
