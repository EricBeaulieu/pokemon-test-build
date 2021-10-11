using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colbur : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Colbur; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Colbur(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Dark && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}