using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBalloon : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.AirBalloon; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new AirBalloon(); }
    public override float AlterDamageTaken(bool superEffective,MoveBase move)
    {
        if(move.Type == ElementType.Ground)
        {
            return 0;
        }
        else
        {
            RemoveItem = true;
            return 1;
        }
    }
}
