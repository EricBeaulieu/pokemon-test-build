using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roseli : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Roseli; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Roseli(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Fairy && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}