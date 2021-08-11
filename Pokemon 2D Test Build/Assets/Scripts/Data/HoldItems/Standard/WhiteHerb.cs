using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteHerb : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.WhiteHerb; } }
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
}
