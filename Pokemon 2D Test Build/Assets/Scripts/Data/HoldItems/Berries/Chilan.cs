using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chilan : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Chilan; } }
    public override float AlterDamageTaken(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Normal)
        {
            holder.removeItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(holder,move, superEffective);
    }
}