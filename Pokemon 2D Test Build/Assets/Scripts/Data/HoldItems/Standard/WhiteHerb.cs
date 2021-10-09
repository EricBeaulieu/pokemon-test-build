using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteHerb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.WhiteHerb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new WhiteHerb(); }
    public override bool RestoresAllLoweredStatsToNormalAfterAttackFinished(Pokemon defendingPokemon)
    {
        foreach (var stat in defendingPokemon.statBoosts)
        {
            if (defendingPokemon.statBoosts[stat.Key] < 0)
            {
                defendingPokemon.RestoreAllLoweredStatBoosts();
                RemoveItem = true;
                return true;
            }
        }
        return false;
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} returned its stats to normal using {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
