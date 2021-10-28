using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrySkin : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.DrySkin; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new DrySkin(); }
    public override string Description()
    {
        return "Restores HP in rain or when hit by Water-type moves. Reduces HP in harsh sunlight, and increases the damage received from Fire-type moves.";
    }
    public override bool AffectsHpByXEachTurnWithWeather(Pokemon pokemon, WeatherEffectID weather)
    {
        if(weather != WeatherEffectID.Sunshine)
        {
            if(weather != WeatherEffectID.Rain)
            {
                return false;
            }
        }
        
        int damage = Mathf.FloorToInt(pokemon.maxHitPoints * HpAmountDeductedByWeather);

        if (damage <= 0)
        {
            damage = 1;
        }

        if (weather == WeatherEffectID.Sunshine)
        {
            pokemon.UpdateHPDamage(damage);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} is hurt from the Sunshine");
        }
        else if(weather == WeatherEffectID.Rain)
        {
            if(pokemon.maxHitPoints == pokemon.currentHitPoints)
            {
                return false;
            }

            int hpHealed = Mathf.Clamp(damage, 1, pokemon.maxHitPoints - pokemon.currentHitPoints);
            pokemon.UpdateHPRestored(hpHealed);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} restored HP using its {Name}!");
        }
        return true;
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if(move.Type == ElementType.Fire)
        {
            if(weather != WeatherEffectID.Sunshine)
            {
                return 1.25f;
            }
            else
            {
                return 2f;
            }
        }
        else if(move.Type == ElementType.Water)
        {
            int hpHealed = Mathf.FloorToInt(defendingPokemon.pokemon.maxHitPoints * 0.25f);
            hpHealed = Mathf.Clamp(hpHealed, 0, defendingPokemon.pokemon.maxHitPoints - defendingPokemon.pokemon.currentHitPoints);
            if(hpHealed == 0)
            {
                defendingPokemon.pokemon.statusChanges.Enqueue($"It Doesn't Affect {defendingPokemon.pokemon.currentName}");
                return 0;
            }
            defendingPokemon.pokemon.UpdateHPRestored(hpHealed);
            defendingPokemon.pokemon.statusChanges.Enqueue($"{defendingPokemon.pokemon.currentName} restored HP using its {Name}!");
            return 0;
        }

        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
