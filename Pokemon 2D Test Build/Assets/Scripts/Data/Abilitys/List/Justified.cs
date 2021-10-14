using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Justified : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Justified; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Justified(); }
    public override string Description()
    {
        return "Being hit by a Dark-type move boosts the Attack stat of the Pokémon, for justice.";
    }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if (attackType == ElementType.Dark)
        {
            return new StatBoost(StatAttribute.Attack, 1);
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
