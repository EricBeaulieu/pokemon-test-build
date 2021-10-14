using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroatSpray : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.ThroatSpray; } }
    public override StatBoost AlterStatAfterUsingSpecificMove(BattleUnit holder, MoveBase move)
    {
        if(move.SoundType == true)
        {
            holder.removeItem = true;
            return new StatBoost(StatAttribute.SpecialAttack,1);
        }
        return base.AlterStatAfterUsingSpecificMove(holder,move);
    }
}
