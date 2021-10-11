using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chilan : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Chilan; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Chilan(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Normal)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}