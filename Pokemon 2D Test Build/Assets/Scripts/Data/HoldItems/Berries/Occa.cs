using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occa : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Occa; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Occa(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if(move.Type == ElementType.Fire && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move,superEffective);
    }
}