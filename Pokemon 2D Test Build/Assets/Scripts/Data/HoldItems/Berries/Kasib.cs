using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kasib : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Kasib; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Kasib(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Ghost && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}