using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRoot : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.BigRoot; } }
    const float hpModifier = 0.3f;
    public override MoveBase AlterUserMoveDetails(BattleUnit holder, MoveBase move)
    {
        if (move.DrainsHP == true)
        {
            move = move.Clone();
            move.AdjustedHPRecovered(hpModifier);
        }
        return base.AlterUserMoveDetails(holder,move);
    }
    public override float HpDrainModifier()
    {
        return hpModifier;
    }
}
