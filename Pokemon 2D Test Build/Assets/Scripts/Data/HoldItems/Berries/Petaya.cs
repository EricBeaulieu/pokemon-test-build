using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petaya : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Petaya; } }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost(StatAttribute.SpecialAttack,1)
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