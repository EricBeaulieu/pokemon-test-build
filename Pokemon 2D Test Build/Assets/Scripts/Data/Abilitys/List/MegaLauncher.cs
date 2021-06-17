using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaLauncher : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.MegaLauncher; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new MegaLauncher(); }
    public override string Description()
    {
        return "Powers up aura and pulse moves.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (currentMove.MoveName.Contains("Pulse") || currentMove.MoveName.Contains("Aura"))
        {
            return 1.5f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
