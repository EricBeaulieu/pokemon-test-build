using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBelt : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.BlackBelt; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new BlackBelt(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Fighting)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
