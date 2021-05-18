using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guts : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Guts; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Guts(); }
    public override string Description()
    {
        return "It's so gutsy that having a status condition boosts the Pokémon's Attack stat.";
    }
    public override float BoostsAStatWhenAffectedWithAStatusCondition(ConditionID iD, StatAttribute benefitialStat)
    {
        if (iD != ConditionID.NA && iD != ConditionID.Frozen && benefitialStat == StatAttribute.Attack)
        {
            return 1.5f;
        }
        return base.BoostsAStatWhenAffectedWithAStatusCondition(iD, benefitialStat);
    }
    public override bool NegatesStatusEffectStatDropFromCondition(ConditionID iD, StatAttribute stat)
    {
        if (iD == ConditionID.Burn && stat == StatAttribute.Attack)
        {
            return true;
        }
        return base.NegatesStatusEffectStatDropFromCondition(iD, stat);
    }
}
