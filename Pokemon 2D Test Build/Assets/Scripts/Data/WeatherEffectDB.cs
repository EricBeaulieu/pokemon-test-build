using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherEffectID
{
    NA, Sunshine, Rain, Hail, Sandstorm
}

public class WeatherEffectDB
{
    public static void Initialization()
    {
        foreach (var kvp in WeatherEffects)
        {
            var weatherEffectID = kvp.Key;
            var weatherEffect = kvp.Value;

            weatherEffect.Id = weatherEffectID;
        }
    }

    public static Dictionary<WeatherEffectID, WeatherEffect> WeatherEffects = new Dictionary<WeatherEffectID, WeatherEffect>()
    {
        {
            WeatherEffectID.Sunshine,
            new WeatherEffect()
            {
                Name = "Sunshine",
                StartMessage = "The Sunlight turned harsh!",
                OnStartDuration = 5,
                OnEndTurn = (BattleSystem battleSystem) =>
                {
                    battleSystem.weatherDuration--;
                    if(battleSystem.weatherDuration > 0)
                    {
                        return "The Sunlight is strong";
                    }

                    battleSystem.RemoveWeatherEffect();
                    return "The sunlight Faded.";
                },
            }

        },
        {
            WeatherEffectID.Rain,
            new WeatherEffect()
            {
                Name = "Rain",
                StartMessage = "It started to Rain",
                OnStartDuration = 5,
                OnEndTurn = (BattleSystem battleSystem) =>
                {
                    battleSystem.weatherDuration--;
                    if(battleSystem.weatherDuration > 0)
                    {
                        return"It continues to Rain";
                    }

                    battleSystem.RemoveWeatherEffect();
                    return "The Rain Stopped.";
                },
            }
        },
        {
            WeatherEffectID.Hail,
            new WeatherEffect()
            {
                Name = "Hail",
                StartMessage = "It started to Hail",
                OnStartDuration = 5,
                OnEndTurn = (BattleSystem battleSystem) =>
                {
                    battleSystem.weatherDuration--;
                    if(battleSystem.weatherDuration > 0)
                    {
                        return "It continues to Hail";
                    }

                    battleSystem.RemoveWeatherEffect();
                    return "The hail stopped.";
                },
                OnEndTurnDamage = (Pokemon pokemon) =>
                {
                    if (pokemon.pokemonBase.IsType(ElementType.Ice))
                    {
                        return;
                    }

                    int damage = pokemon.maxHitPoints/16;

                    if(damage <=0)
                    {
                        damage = 1;
                    }

                    pokemon.UpdateHP(damage);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} was hit by hail");
                },
            }
        },
        {
            WeatherEffectID.Sandstorm,
            new WeatherEffect()
            {
                Name = "Sandstorm",
                StartMessage = "A Sandstorm kicked up",
                OnStartDuration = 5,
                OnEndTurn = (BattleSystem battleSystem) =>
                {
                    battleSystem.weatherDuration--;
                    if(battleSystem.weatherDuration > 0)
                    {
                        return "The Sandstorm rages";
                    }

                    battleSystem.RemoveWeatherEffect();
                    return "The Sandstorm subsided.";
                },
                OnEndTurnDamage = (Pokemon pokemon) =>
                {
                    if(pokemon.pokemonBase.IsType(ElementType.Rock)||pokemon.pokemonBase.IsType(ElementType.Steel)||pokemon.pokemonBase.IsType(ElementType.Ground))
                    {
                        return;
                    }

                    int damage = pokemon.maxHitPoints/16;

                    if(damage <=0)
                    {
                        damage = 1;
                    }

                    pokemon.UpdateHP(damage);
                    pokemon.statusChanges.Enqueue($"{pokemon.currentName} was hit by the sandstorm");
                },
            }
        }
    };
}
