using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charti : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Charti; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Charti(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Rock && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}