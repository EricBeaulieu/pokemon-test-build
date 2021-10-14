using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wacan : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Wacan; } }
    public override float AlterDamageTaken(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Electric && superEffective == true)
        {
            holder.removeItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(holder,move, superEffective);
    }
}