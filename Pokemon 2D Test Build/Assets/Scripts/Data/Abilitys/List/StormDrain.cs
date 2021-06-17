using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDrain : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.StormDrain; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new StormDrain(); }
    public override string Description()
    {
        return "Draws in all Water-type moves. Instead of being hit by Water-type moves, it boosts its Sp. Atk.";
    }
    public override float AlterDamageTaken(Pokemon defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.Type == ElementType.Water)
        {
            return 0;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if(attackType == ElementType.Water)
        {
            return new StatBoost() { stat = StatAttribute.SpecialAttack, boost = 1 };
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
