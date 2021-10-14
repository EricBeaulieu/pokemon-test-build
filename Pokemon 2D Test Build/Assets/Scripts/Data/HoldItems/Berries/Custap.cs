using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custap : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Custap; } }
    public override int AdjustSpeedPriorityTurn(BattleUnit holder)
    {
        float pokemonHealthPercentage = (float)holder.pokemon.currentHitPoints / (float)holder.pokemon.maxHitPoints;
        if (pokemonHealthPercentage <= StandardBerryUseage(holder.pokemon))
        {
            holder.removeItem = true;
            return 1;
        }
        return base.AdjustSpeedPriorityTurn(holder);
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} raised their attacks priority using the {GlobalTools.SplitCamelCase(BerryId.ToString())} berry!";
    }
}