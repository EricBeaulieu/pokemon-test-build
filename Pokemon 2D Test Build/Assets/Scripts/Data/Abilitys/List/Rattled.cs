using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rattled : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Rattled; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Rattled(); }
    public override string Description()
    {
        return "Dark-, Ghost-, and Bug-type moves scare the Pokémon and boost its Speed stat.";
    }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if (attackType == ElementType.Dark|| attackType == ElementType.Ghost || attackType == ElementType.Bug)
        {
            return new StatBoost() { stat = StatAttribute.Speed, boost = 1 };
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
