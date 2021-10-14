using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRod : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.LightningRod; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new LightningRod(); }
    public override string Description()
    {
        return "The Pokémon draws in all Electric-type moves. Instead of being hit by Electric-type moves, it boosts its Sp. Atk.";
    }
    public override float AlterDamageTaken(Pokemon defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.Type == ElementType.Electric)
        {
            return 0;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if (attackType == ElementType.Electric)
        {
            return new StatBoost(StatAttribute.SpecialAttack,1);
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
