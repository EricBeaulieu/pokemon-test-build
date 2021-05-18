using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareBoost : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.FlareBoost; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new FlareBoost(); }
    public override string Description()
    {
        return "Powers up special attacks when the Pok�mon is burned.";
    }
    public override float BoostsAStatWhenAffectedWithAStatusCondition(ConditionID iD, StatAttribute benefitialStat)
    {
        if (iD == ConditionID.Burn && benefitialStat == StatAttribute.SpecialAttack)
        {
            return 1.5f;
        }
        return base.BoostsAStatWhenAffectedWithAStatusCondition(iD, benefitialStat);
    }
}
