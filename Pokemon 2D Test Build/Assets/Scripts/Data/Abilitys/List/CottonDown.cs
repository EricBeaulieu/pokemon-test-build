using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottonDown : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.CottonDown; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new CottonDown(); }
    public override string Description()
    {
        return "When the Pok�mon is hit by an attack, it scatters cotton fluff around and lowers the Speed stat of all Pok�mon except itself.";
    }
    public override StatBoost AlterStatAfterTakingDamage(MoveBase move)
    {
        return new StatBoost() { stat = StatAttribute.Speed, boost = -1 };
    }
}
