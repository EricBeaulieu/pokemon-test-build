using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maranga : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Maranga; } }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost(StatAttribute.SpecialDefense,1)
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder, MoveBase move, bool superEffective)
    {
        float pokemonHealthPercentage = (float)holder.pokemon.currentHitPoints / (float)holder.pokemon.maxHitPoints;
        if (move.MoveType == MoveType.Special)
        {
            holder.removeItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(holder, move, superEffective);
    }
}