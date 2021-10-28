using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapSipper : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SapSipper; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SapSipper(); }
    public override string Description()
    {
        return "Boosts the Attack stat if hit by a Grass-type move instead of taking damage.";
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (move.Type == ElementType.Grass)
        {
            return 0;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if (attackType == ElementType.Grass)
        {
            return new StatBoost(StatAttribute.Attack,1);
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
