using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroatSpray : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ThroatSpray; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ThroatSpray(); }
    public override StatBoost AlterStatAfterUsingSpecificMove(MoveBase move)
    {
        if(move.SoundType == true)
        {
            RemoveItem = true;
            return new StatBoost() { stat = StatAttribute.SpecialAttack, boost = 1 };
        }
        return base.AlterStatAfterUsingSpecificMove(move);
    }
}
