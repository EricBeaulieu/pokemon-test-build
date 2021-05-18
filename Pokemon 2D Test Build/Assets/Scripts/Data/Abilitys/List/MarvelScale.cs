using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarvelScale : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.MarvelScale; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new MarvelScale(); }
    public override string Description()
    {
        return "The Pokémon's marvelous scales boost the Defense stat if it has a status condition.";
    }
    public override float BoostsAStatWhenAffectedWithAStatusCondition(ConditionID iD, StatAttribute benefitialStat)
    {
        if (iD != ConditionID.NA && benefitialStat == StatAttribute.Defense)
        {
            return 1.5f;
        }
        return base.BoostsAStatWhenAffectedWithAStatusCondition(iD, benefitialStat);
    }
}
