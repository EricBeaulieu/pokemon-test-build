using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Intimidate; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Intimidate(); }
    public override string Description()
    {
        return "The Pokémon intimidates opposing Pokémon upon entering battle, lowering their Attack stat.";
    }
    public override StatBoost OnEntryLowerStat(AbilityID opposingAbility)
    {
        if(opposingAbility == AbilityID.InnerFocus || opposingAbility == AbilityID.Oblivious || opposingAbility == AbilityID.Scrappy || opposingAbility == AbilityID.OwnTempo)
        {
            return null;
        }
        return new StatBoost() { stat = StatAttribute.Attack, boost = -1 };
    }
}
