using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamEngine : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SteamEngine; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SteamEngine(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Defense stat sharply when hit by a Water-type move.";
    }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if (attackType == ElementType.Water|| attackType == ElementType.Fire)
        {
            return new StatBoost(StatAttribute.Speed,6);
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
