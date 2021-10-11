using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coba : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Coba; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Coba(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Flying && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}