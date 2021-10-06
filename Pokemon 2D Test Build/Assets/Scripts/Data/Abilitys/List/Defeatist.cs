using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defeatist : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Defeatist; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Defeatist(); }
    public override string Description()
    {
        return "Halves the Pokémon's Attack and Sp. Atk stats when its HP becomes half or less.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (attackingPokemon.currentHitPoints <= (attackingPokemon.maxHitPoints/2))
        {
            return 0.5f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove, weather);
    }
}
