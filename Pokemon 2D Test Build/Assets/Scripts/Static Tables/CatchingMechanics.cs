using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CatchingMechanics
{

    static float StatusModifier(ConditionID currentCondition)
    {
        switch (currentCondition)
        {
            case ConditionID.poison:
                return 1.5f;
            case ConditionID.burn:
                return 1.5f;
            case ConditionID.sleep:
                return 2.5f;
            case ConditionID.paralyzed:
                return 1.5f;
            case ConditionID.frozen:
                return 2.5f;
            case ConditionID.toxicPoison:
                return 1.5f;
            default:
                return 1;
        }
    }

    static float captureRate(PokeballCaptureID pokeballCaptureID, Pokemon currentPokemon)
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
                //To be redone later
                //A Ball that works better on the ocean floor and on Pokémon on water
                //If used out of water then * 1.If battle field is water then * 3.5
                return 1f;
            case PokeballCaptureID.Nest:

                break;
            case PokeballCaptureID.Repeat:
                //	A Ball that works better on Pokémon previously captured.
                //If the Pokémon hasn't been in your Pokédex then * 1. If it has then * 3.
                return 1f;
            case PokeballCaptureID.Timer:
                return 1f;
        }
        return 1f;
    }

    public static int CatchRate(Pokemon currentPokemon,PokeballItem pokeball)
    {
        if(pokeball.CaptureRate == PokeballCaptureID.Master)
        {
            return 4;
        }

        float pokeballCaptureRate = captureRate(pokeball.CaptureRate,currentPokemon);

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
            if (rnd <= catchValue)
            {
                break;
            }
            shakeCount++;
        }
        return shakeCount;
    }
}
