using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payapa : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Payapa; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Payapa(); }
    public override float AlterDamageTaken(MoveBase move, bool superEffective)
    {
        if (move.Type == ElementType.Psychic && superEffective == true)
        {
            RemoveItem = true;
            return 0.5f;
        }
        return base.AlterDamageTaken(move, superEffective);
    }
}