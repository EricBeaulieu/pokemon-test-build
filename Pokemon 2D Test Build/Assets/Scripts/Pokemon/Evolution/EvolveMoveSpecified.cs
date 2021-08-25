using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolveMoveSpecified : EvolutionBase
{
    [SerializeField] MoveBase moveRequired;

    public override bool CanEvolve(Pokemon pokemon,ItemBase item)
    {
        return (pokemon.moves.Exists(x => x.moveBase == moveRequired));
    }
}
