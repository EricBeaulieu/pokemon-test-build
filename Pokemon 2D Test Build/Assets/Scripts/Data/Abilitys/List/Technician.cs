using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Technician : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Technician; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Technician(); }
    public override string Description()
    {
        return "Powers up the Pokémon's weaker moves.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove)
    {
        if (currentMove.MovePower <= 60)
        {
            return 1.5f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove);
    }
}
