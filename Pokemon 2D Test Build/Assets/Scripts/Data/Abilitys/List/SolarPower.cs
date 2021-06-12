using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPower : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SolarPower; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SolarPower(); }
    public override string Description()
    {
        return "Boosts the Sp. Atk stat in harsh sunlight, but HP decreases every turn.";
    }
    public override float AlterStatDuringWeatherEffect(WeatherEffectID iD, StatAttribute statAffected)
    {
        if (iD == WeatherEffectID.Sunshine && statAffected == StatAttribute.SpecialAttack)
        {
            return 1.5f;
        }
        return base.AlterStatDuringWeatherEffect(iD, statAffected);
    }
    public override bool AffectsHpByXEachTurnWithWeather(Pokemon pokemon, WeatherEffectID weather)
    {
        if (weather != WeatherEffectID.Sunshine)
        {
            return false;
        }

        int damage = Mathf.FloorToInt(pokemon.maxHitPoints * HpAmountDeductedByWeather);

        if (damage <= 0)
        {
            damage = 1;
        }

        pokemon.UpdateHPDamage(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt from the Sunshine");

        return true;
    }
}
