using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageModifiers
{
    public static float CriticalHitModifier { get; } = 1.5f;

    static float[,] _typeChart =
    {

        //Attack Type -> Bug    Dark    Dragon  Elect   Fairy   Fight    Fire    Flying  Ghost   Grass   Ground  Ice     Normal  Poison  Psychic Rock    Steel   Water

        //DefenseType

        /*Bug*/         {1,     1,      1,      1,      1,      0.5f,   2,      2,      1,      0.5f,   0.5f,   1,      1,      1,      1,      2,      1,      1},
        /*Dark*/        {2,     0.5f,   1,      1,      2,      2,      1,      1,      0.5f,   1,      1,      1,      1,      1,      0,      1,      1,      1},
        /*Dragon*/      {1,     1,      2,      0.5f,   2,      1,      0.5f,   1,      1,      0.5f,   1,      2,      1,      1,      1,      1,      1,      0.5f},   
        /*Electric*/    {1,     1,      1,      0.5f,   1,      1,      1,      0.5f,   1,      1,      2,      1,      1,      1,      1,      1,      0.5f,   1},
        /*Fairy*/       {0.5f,  0.5f,   0,      1,      1,      0.5f,   1,      1,      1,      1,      1,      1,      1,      2,      1,      1,      2,      1},
        /*Fighting*/    {0.5f,  0.5f,   1,      1,      2,      1,      1,      2,      1,      1,      1,      1,      1,      1,      2,      0.5f,   1,      1},
        /*Fire*/        {0.5f,  1,      1,      1,      0.5f,   1,      0.5f,   1,      1,      0.5f,   2,      0.5f,   1,      1,      1,      2,      0.5f,   2},
        /*Flying*/      {0.5f,  1,      1,      2,      1,      0.5f,   1,      1,      1,      0.5f,   0,      2,      1,      1,      1,      2,      1,      1},
        /*Ghost*/       {0.5f,  2,      1,      1,      1,      0,      1,      1,      2,      1,      1,      1,      0,      0.5f,   1,      1,      1,      1},
        /*Grass*/       {2,     1,      1,      0.5f,   1,      1,      2,      2,      1,      0.5f,   0.5f,   2,      1,      2,      1,      1,      1,      0.5f},
        /*Ground*/      {1,     1,      1,      0,      1,      1,      1,      1,      1,      2,      1,      2,      1,      0.5f,   1,      0.5f,   1,      2},
        /*Ice*/         {1,     1,      1,      1,      1,      2,      2,      1,      1,      1,      1,      0.5f,   1,      1,      1,      2,      2,      1},
        /*Normal*/      {1,     1,      1,      1,      1,      2,      1,      1,      0,      1,      1,      1,      1,      1,      1,      1,      1,      1},
        /*Poison*/      {0.5f,  1,      1,      1,      1,      0.5f,   1,      1,      1,      0.5f,   0,      1,      1,      0.5f,   2,      1,      1,      1},
        /*Psychic*/     {2,     2,      1,      1,      1,      0.5f,   1,      1,      2,      1,      1,      1,      1,      1,      0.5f,   1,      1,      1},
        /*Rock*/        {1,     1,      1,      1,      1,      2,      0.5f,   0.5f,   1,      2,      2,      1,      0.5f,   0.5f,   1,      1,      1,      2},
        /*Steel*/       {0.5f,  1,      0.5f,   1,      0.5f,   2,      2,      0.5f,   1,      0.5f,   2,      0.5f,   0.5f,   0,      0.5f,   0.5f,   0.5f,   1},
        /*Water*/       {1,     1,      1,      2,      1,      1,      0.5f,   1,      1,      2,      1,      0.5f,   1,      1,      1,      1,      0.5f,   0.5f}
    };

    public static float StandardRandomAttackPowerModifier()
    {
        return Random.Range(0.85f, 1f);
    }

    public static float SameTypeAttackBonus(MoveBase move,PokemonBase attackingPokemon)
    {
        if(attackingPokemon.IsType(move.Type))
        {
            return 1.5f;
        }
        return 1f;
    }

    public static float TypeChartEffectiveness(PokemonBase defendingPokemon,ElementType attackType)
    {
        float damageMultiplier = 1f;

        damageMultiplier *= _typeChart[(int)defendingPokemon.pokemonType1, (int)attackType];
        
        if(defendingPokemon.pokemonType2 != ElementType.NA)
        {
            damageMultiplier *= _typeChart[(int)defendingPokemon.pokemonType2, (int)attackType];
        }

        return damageMultiplier;
    }

    public static float WeatherConditionModifiers(WeatherEffectID weather,MoveBase currentMove)
    {
        if(weather == WeatherEffectID.Sunshine)
        {
            if(currentMove.Type == ElementType.Fire)
            {
                return 1.5f;
            }
            else if(currentMove.Type == ElementType.Water)
            {
                return 0.5f;
            }
        }
        else if(weather == WeatherEffectID.Rain)
        {
            if (currentMove.Type == ElementType.Water)
            {
                return 1.5f;
            }
            else if (currentMove.Type == ElementType.Fire)
            {
                return 0.5f;
            }
        }

        return 1f;
    }

    public static float SandStormSpecialDefenseBonus(WeatherEffectID weather, Pokemon defendingPokemon, MoveBase move)
    {
        if(weather == WeatherEffectID.Sandstorm && defendingPokemon.pokemonBase.IsType(ElementType.Rock) && move.MoveType == MoveType.Special)
        {
            return 0.5f;
        }

        return 1f;
    }
}
