using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SpeedBoost; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SpeedBoost(); }
    public override string Description()
    {
        return "Its Speed stat is boosted every turn.";
    }
    public override StatBoost AlterStatAtTurnEnd()
    {
        return new StatBoost(StatAttribute.Speed,1);
    }
}
