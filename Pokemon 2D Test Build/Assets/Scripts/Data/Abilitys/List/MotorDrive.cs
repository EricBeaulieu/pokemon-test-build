using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorDrive : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.MotorDrive; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new MotorDrive(); }
    public override string Description()
    {
        return "Boosts its Speed stat if hit by an Electric-type move instead of taking damage.";
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
            return new StatBoost() { stat = StatAttribute.Speed, boost = 1 };
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
