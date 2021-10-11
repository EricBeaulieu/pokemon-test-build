using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuca : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Shuca; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Shuca(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Ground && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}