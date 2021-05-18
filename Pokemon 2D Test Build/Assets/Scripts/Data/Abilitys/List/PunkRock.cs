using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunkRock : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.PunkRock; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new PunkRock(); }
    public override string Description()
    {
        return "Boosts the power of sound-based moves. The Pokémon also takes half the damage from these kinds of moves.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove)
    {
        if (currentMove.SoundType == true)
        {
            return 1.3f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove);
    }
}
