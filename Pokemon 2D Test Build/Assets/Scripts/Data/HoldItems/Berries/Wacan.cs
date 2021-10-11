using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wacan : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Wacan; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Wacan(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Electric && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}