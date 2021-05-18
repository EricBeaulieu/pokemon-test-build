using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicBoost : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.ToxicBoost; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new ToxicBoost(); }
    public override string Description()
    {
        return "Powers up physical attacks when the Pokémon is poisoned.";
    }
    public override float BoostsAStatWhenAffectedWithAStatusCondition(ConditionID iD, StatAttribute benefitialStat)
    {
        if (iD == ConditionID.Poison || iD == ConditionID.ToxicPoison && benefitialStat == StatAttribute.Attack)
        {
            return 1.5f;
        }
        return base.BoostsAStatWhenAffectedWithAStatusCondition(iD, benefitialStat);
    }
}
