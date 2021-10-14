using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBattery : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.CellBattery; } }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost(StatAttribute.Attack,1)
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Electric)
        {
            holder.removeItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(holder,move,superEffective);
    }
}
