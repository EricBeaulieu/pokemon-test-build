using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kebia : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Kebia; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Kebia(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Poison && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}