using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ganlon : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Ganlon; } }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost(StatAttribute.Defense,1)
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder, MoveBase move, bool superEffective)
    {
        float pokemonHealthPercentage = (float)holder.pokemon.currentHitPoints / (float)holder.pokemon.maxHitPoints;
        if (pokemonHealthPercentage <= StandardBerryUseage(holder.pokemon))
        {
            holder.removeItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(holder, move, superEffective);
    }
}