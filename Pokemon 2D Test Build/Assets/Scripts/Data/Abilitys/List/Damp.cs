using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damp : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Damp; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Damp(); }
    public override string Description()
    {
        return "Prevents the use of explosive moves, such as Self-Destruct, by dampening its surroundings.";
    }
    public override bool PreventsTheUseOfSpecificMoves(Pokemon attackingPokemon, MoveBase move)
    {
        if(move.RecoilPercentage >= 100)
        {
            attackingPokemon.statusChanges.Enqueue($"{attackingPokemon.currentName} cannot use {move.MoveName}");
            return true;
        }
        return base.PreventsTheUseOfSpecificMoves(attackingPokemon,move);
    }
}
