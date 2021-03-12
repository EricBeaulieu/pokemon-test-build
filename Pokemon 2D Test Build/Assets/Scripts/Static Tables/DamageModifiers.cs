using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageModifiers
{
    static float[,] TypeChart =
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
        /*Poison*/      {0.5f,  1,      1,      1,      1,      0.5f,   1,      1,      1,      0.5f,   0,      1,      1,      0.5f,   0,      1,      1,      1},
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
        if(move.Type == attackingPokemon.pokemonType1 || move.Type == attackingPokemon.pokemonType2)
        {
            return 1.5f;
        }
        return 1f;
    }

    public static float TypeChartEffectiveness(PokemonBase defendingPokemon,ElementType attackType)
    {
        float damageMultiplier = 1f;

        damageMultiplier *= TypeChart[(int)defendingPokemon.pokemonType1, (int)attackType];
        
        if(defendingPokemon.pokemonType2 != ElementType.NA)
        {
            damageMultiplier *= TypeChart[(int)defendingPokemon.pokemonType2, (int)attackType];
        }

        return damageMultiplier;
    }
}
