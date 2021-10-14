using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liechi : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Liechi; } }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost(StatAttribute.Attack,1)
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder, MoveBase move, bool superEffective)
    {
        float pokemonHealthPercentage = (float)holder.pokemon.currentHitPoints / (float)holder.pokemon.maxHitPoints;
        if (pokemonHealthPercentage <= StandardBerryUseage(holder.pokemon))
        {
            holder.removeItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(holder,move, superEffective);
    }
}