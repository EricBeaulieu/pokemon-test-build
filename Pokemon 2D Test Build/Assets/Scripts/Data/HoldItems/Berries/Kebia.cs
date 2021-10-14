using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kebia : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Kebia; } }
    public override float AlterDamageTaken(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Poison && superEffective == true)
        {
            holder.removeItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(holder,move, superEffective);
    }
}