using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFang : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.DragonFang; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new DragonFang(); }
    public override MoveBase AlterUserMoveDetails(MoveBase move)
    {
        if (move.Type == ElementType.Dragon)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.2f);
        }
        return base.AlterUserMoveDetails(move);
    }
}
