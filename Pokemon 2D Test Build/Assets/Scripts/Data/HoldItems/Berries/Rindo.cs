using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rindo : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Rindo; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Rindo(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Grass && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}