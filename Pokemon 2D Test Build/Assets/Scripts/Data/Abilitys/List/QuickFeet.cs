using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFeet : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.QuickFeet; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new QuickFeet(); }
    public override string Description()
    {
        return "Boosts the Speed stat if the Pokémon has a status condition.";
    }
    public override float BoostsAStatWhenAffectedWithAStatusCondition(ConditionID iD, StatAttribute benefitialStat)
    {
        if (iD != ConditionID.NA && benefitialStat == StatAttribute.Speed)
        {
            return 1.5f;
        }
        return base.BoostsAStatWhenAffectedWithAStatusCondition(iD, benefitialStat);
    }
    public override bool NegatesStatusEffectStatDropFromCondition(ConditionID iD, StatAttribute stat)
    {
        if (iD == ConditionID.Paralyzed && stat == StatAttribute.Speed)
        {
            return true;
        }
        return base.NegatesStatusEffectStatDropFromCondition(iD, stat);
    }
}
