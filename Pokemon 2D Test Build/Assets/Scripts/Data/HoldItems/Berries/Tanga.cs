using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanga : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Tanga; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Tanga(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Bug && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}