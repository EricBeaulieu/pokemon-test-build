using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbBulb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.AbsorbBulb; } }
    List<StatBoost> statBoosts = new List<StatBoost>()
    {
        new StatBoost(StatAttribute.SpecialAttack,1)
    };
    public override List<StatBoost> AlterStatAfterTakingDamageFromCertainType(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if(move.Type == ElementType.Water)
        {
            holder.removeItem = true;
            return statBoosts;
        }
        return base.AlterStatAfterTakingDamageFromCertainType(holder,move,superEffective);
    }
}
