using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRoot : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.BigRoot; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new BigRoot(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.DrainsHP == true)
        {
            move = move.Clone();
            move.AdjustedHPRecovered(0.3f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
