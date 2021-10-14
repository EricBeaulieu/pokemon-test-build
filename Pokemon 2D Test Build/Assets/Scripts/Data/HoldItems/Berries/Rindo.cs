using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rindo : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Rindo; } }
    public override float AlterDamageTaken(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Grass && superEffective == true)
        {
            holder.removeItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(holder,move, superEffective);
    }
}