using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuminousMoss : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LuminousMoss; } }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost(StatAttribute.SpecialDefense,1)
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Water)
        {
            holder.removeItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(holder,move,superEffective);
    }
}
