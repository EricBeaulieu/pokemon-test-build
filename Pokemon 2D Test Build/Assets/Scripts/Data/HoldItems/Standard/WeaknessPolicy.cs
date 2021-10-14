using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessPolicy : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.WeaknessPolicy; } }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost(StatAttribute.Attack,2),
        new StatBoost(StatAttribute.SpecialAttack,2)
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if (superEffective == true)
        {
            holder.removeItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(holder,move, superEffective);
    }
}
