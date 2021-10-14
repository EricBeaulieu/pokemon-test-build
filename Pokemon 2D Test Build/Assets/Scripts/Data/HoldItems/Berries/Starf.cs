using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starf : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Starf; } }
    List<StatBoost> statBoosts;
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder, MoveBase move, bool superEffective)
    {
        float pokemonHealthPercentage = (float)holder.pokemon.currentHitPoints / (float)holder.pokemon.maxHitPoints;
        if (pokemonHealthPercentage <= StandardBerryUseage(holder.pokemon))
        {
            statBoosts = RandomizedStatBoost();
            holder.removeItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(holder, move, superEffective);
    }
    List<StatBoost> RandomizedStatBoost()
    {
        List<StatBoost> returnedStatBoost = new List<StatBoost>();
        StatAttribute statAttribute = (StatAttribute)Random.Range((int)StatAttribute.Attack, (int)StatAttribute.CriticalHitRatio);
        returnedStatBoost.Add(new StatBoost(statAttribute, 2));
        return returnedStatBoost;
    }
}