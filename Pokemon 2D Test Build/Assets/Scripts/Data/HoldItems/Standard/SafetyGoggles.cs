using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyGoggles : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SafetyGoggles; } }
    public override float AlterDamageTaken(BattleUnit holder, MoveBase move, bool superEffective)
    {
        if (move.MoveName.Contains("Powder") || move.MoveName.Contains("Spore"))
        {
            return 0;
        }
        return base.AlterDamageTaken(holder,move,superEffective);
    }
    public override bool ProtectsHolderFromWeatherConditions()
    {
        return true;
    }
}
