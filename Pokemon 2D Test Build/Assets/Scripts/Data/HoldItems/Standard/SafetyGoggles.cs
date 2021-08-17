using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyGoggles : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.SafetyGoggles; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SafetyGoggles(); }
    public override float AlterDamageTaken(MoveBase move)
    {
        if (move.MoveName.Contains("Powder") || move.MoveName.Contains("Spore"))
        {
            return 0;
        }
        return base.AlterDamageTaken(move);
    }
    public override bool ProtectsHolderFromWeatherConditions()
    {
        return true;
    }
}
