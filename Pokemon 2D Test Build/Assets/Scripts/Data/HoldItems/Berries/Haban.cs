using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haban : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Haban; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Haban(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Dragon && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}