using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yache : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Yache; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Yache(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Ice && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}