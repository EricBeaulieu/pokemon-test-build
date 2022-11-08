using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CatchingMechanics
{
    static float heavyBallWeightClassSmallmin = 451.1f;
    static float heavyBallWeightClassMedmin = 677.3f;
    static float heavyBallWeightClassLargemin = 903f;

    static float StatusModifier(ConditionID currentCondition)
    {
        switch (currentCondition)
        {
            case ConditionID.Poison:
                return 1.5f;
            case ConditionID.Burn:
                return 1.5f;
            case ConditionID.Sleep:
                return 2.5f;
            case ConditionID.Paralyzed:
                return 1.5f;
            case ConditionID.Frozen:
                return 2.5f;
            case ConditionID.ToxicPoison:
                return 1.5f;
            default:
                return 1;
        }
    }

    static float captureRate(PokeballCaptureID pokeballCaptureID, Pokemon currentPokemon, Pokemon playersPokemon,int battleDuration)
    {
        switch (pokeballCaptureID)
        {
            case PokeballCaptureID.Great:
                return 1.5f;
            case PokeballCaptureID.Ultra:
                return 2f;
            case PokeballCaptureID.Net:
                if(currentPokemon.pokemonBase.IsType(ElementType.Bug)|| currentPokemon.pokemonBase.IsType(ElementType.Water))
                {
                    return 3.5f;
                }
                else
                {
                    return 1;
                }
            case PokeballCaptureID.Dive:
                if(GameManager.instance.GetPlayerController.isSurfing)
                {
                    return 3.5f;
                }
                return 1f;
            case PokeballCaptureID.Nest:
                return (8 - 0.2f * (currentPokemon.currentLevel - 1));
            case PokeballCaptureID.Repeat:
                //A Ball that works better on Pokémon previously captured.
                //If the Pokémon hasn't been in your Pokédex then * 1. If it has then * 3.
                return 1f;
            case PokeballCaptureID.Timer:
                float modifier = 1 + (0.3f * battleDuration);
                if(modifier >= 4)
                {
                    modifier = 4;
                }
                return modifier;
            case PokeballCaptureID.Quick:
                if(battleDuration == 0)
                {
                    return 4f;
                }
                return 1f;
            case PokeballCaptureID.Dusk:
                return 1f;
            case PokeballCaptureID.Fast:
                if(currentPokemon.pokemonBase.speed >=100)
                {
                    return 4f;
                }
                return 1f;
            case PokeballCaptureID.Heavy:
                if(currentPokemon.pokemonBase.weight < heavyBallWeightClassSmallmin)
                {
                    return 0.8f;
                }
                else if(currentPokemon.pokemonBase.weight >= heavyBallWeightClassSmallmin && currentPokemon.pokemonBase.weight < heavyBallWeightClassMedmin)
                {
                    return 1.2f;
                }
                else if (currentPokemon.pokemonBase.weight >= heavyBallWeightClassMedmin && currentPokemon.pokemonBase.weight < heavyBallWeightClassLargemin)
                {
                    return 1.3f;
                }
                return 1.4f;
            case PokeballCaptureID.Level:
                float levelDif = currentPokemon.currentLevel / playersPokemon.currentLevel;
                if(levelDif < 1 && levelDif >= 0.5)
                {
                    return 2f;
                }
                else if (levelDif < 0.5 && levelDif >= 0.25)
                {
                    return 4f;
                }
                else if (levelDif < 0.25)
                {
                    return 8f;
                }
                return 1f;
            case PokeballCaptureID.Love:
                if(BattleSystem.CheckIfTargetCanBeInflatuated(currentPokemon, playersPokemon,true))
                {
                    return 8f;
                }
                return 1f;
            case PokeballCaptureID.Lure:
                if(BattleSystem.CaughtByRod == true)
                {
                    return 5f;
                }
                return 1f;
            case PokeballCaptureID.Moon:
                if(PokemonWhoEvolveWithMoonstone(currentPokemon.pokemonBase.GetPokedexNumber()) == true)
                {
                    return 4f;
                }
                return 1f;
        }
        return 1f;
    }

    public static int CatchRate(Pokemon currentPokemon,Pokemon playersPokemon,PokeballItem pokeball,int battleDuration)
    {
        if(pokeball.PokeballId == PokeballCaptureID.Master)
        {
            return 4;
        }

        float pokeballCaptureRate = captureRate(pokeball.PokeballId,currentPokemon,playersPokemon,battleDuration);

        float catchValue = (((3 * currentPokemon.maxHitPoints - 2 * currentPokemon.currentHitPoints)
            * (currentPokemon.pokemonBase.GetCatchRate() * pokeballCaptureRate) / (3 * currentPokemon.maxHitPoints)) 
            * StatusModifier(currentPokemon.GetCurrentStatus()));

        if(catchValue >= 255)
        {
            return 4;
        }

        //(2^20 - 2^4) / Mathf.Sqrt(Mathf.Sqrt((2^24 - 2^16) / catchValue));
        catchValue = 1048560 / Mathf.Sqrt(Mathf.Sqrt(16711680 / catchValue));

        int shakeCount = 0;
        while(shakeCount < 4)
        {
            int rnd = Random.Range(0, 65535);
            if (rnd >= catchValue)
            {
                break;
            }
            shakeCount++;
        }
        return shakeCount;
    }

    public static void ApplyEffectsAfterCatch(PokeballCaptureID pokeballCaptureID,Pokemon capturedPokemon)
    {
        switch (pokeballCaptureID)
        {
            case PokeballCaptureID.Luxury://For each 1 Happiness point it normally would earn, it instead earns 2, thus doubling all Happiness earned
                break;
            case PokeballCaptureID.Heal:
                capturedPokemon.FullyHeal();
                break;
            case PokeballCaptureID.Friend://The Pokémon's Base Happiness is 200 instead of 70
                break;
        }
    }

    static bool PokemonWhoEvolveWithMoonstone(int pokedexNumber)
    {
        if (pokedexNumber >= 29 && pokedexNumber >= 36)//Nidoran Family + Clefairy
        {
            return true;
        }
        else if (pokedexNumber >= 39 && pokedexNumber >= 40)//Jigglypuff Family
        {
            return true;
        }
        else if (pokedexNumber >= 173 && pokedexNumber >= 174)//Cleffa + Igglybuff
        {
            return true;
        }
        else if (pokedexNumber >= 300 && pokedexNumber >= 301)//Skitty Family
        {
            return true;
        }
        else if (pokedexNumber >= 517 && pokedexNumber >= 518)//Munna Family
        {
            return true;
        }
        return false;
    }
}
