using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandForce : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SandForce; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SandForce(); }
    public override string Description()
    {
        return "Boosts the power of Rock-, Ground-, and Steel-type moves in a sandstorm.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove,WeatherEffectID weather)
    {
        if(weather == WeatherEffectID.Sandstorm)
        {
            if(currentMove.Type == ElementType.Ground|| currentMove.Type == ElementType.Rock || currentMove.Type == ElementType.Steel)
            {
                return 1.33f;
            }
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
