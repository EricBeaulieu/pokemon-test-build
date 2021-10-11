using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Babiri : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Babiri; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Babiri(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Steel && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}