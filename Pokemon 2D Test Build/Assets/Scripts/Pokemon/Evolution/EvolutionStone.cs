using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EvolutionStone : EvolutionBase
{
    [SerializeField] EvolutionStoneBase evolutionStone;

    public override bool CanEvolve(Pokemon pokemon,ItemBase currentStone)
    {
        foreach (EvolutionStone evolution in pokemon.pokemonBase.EvolutionsByStone)
        {
            if(evolutionStone == currentStone)
            {
                return true;
            }
        }
        return false;
    }

    public EvolutionStoneBase RequiredStone
    {
        get { return evolutionStone; }
    }
}
