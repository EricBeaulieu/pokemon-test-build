using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFire : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.FlashFire; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new FlashFire(); }
    public override string Description()
    {
        return "Powers up the Pokémon's Fire-type moves if it's hit by one.";
    }
    bool charged = false;
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if(charged == false)
        {
            return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove, weather);
        }

        if(currentMove.Type != ElementType.Fire)
        {
            return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove, weather);
        }

        charged = false;
        return 1.5f;
    }
    public override float AlterDamageTaken(Pokemon defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if(move.Type == ElementType.Fire)
        {
            charged = true;
            defendingPokemon.statusChanges.Enqueue($"{defendingPokemon.currentName}'s {Name} prevents damage");
            return 0f;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
