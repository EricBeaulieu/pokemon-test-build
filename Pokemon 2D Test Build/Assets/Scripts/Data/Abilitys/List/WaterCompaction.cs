using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCompaction : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.WaterCompaction; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new WaterCompaction(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Defense stat sharply when hit by a Water-type move.";
    }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if (attackType == ElementType.Water)
        {
            return new StatBoost(StatAttribute.Defense,2);
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
