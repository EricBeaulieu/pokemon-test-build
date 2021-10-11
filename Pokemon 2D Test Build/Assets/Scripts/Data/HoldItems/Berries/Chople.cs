using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chople : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Chople; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Chople(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Fighting && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}