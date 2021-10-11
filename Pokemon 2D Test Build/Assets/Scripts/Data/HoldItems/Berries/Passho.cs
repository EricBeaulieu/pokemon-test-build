using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passho : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Passho; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Passho(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Water && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}