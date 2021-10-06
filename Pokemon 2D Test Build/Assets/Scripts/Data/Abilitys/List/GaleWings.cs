using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaleWings : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.GaleWings; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new GaleWings(); }
    public override string Description()
    {
        return "Gives priority to Flying-type moves when the Pokémon's HP is full.";
    }
    public override int AdjustSpeedPriorityOfMove(Pokemon attackingPokemon, MoveBase move)
    {
        if(attackingPokemon.currentHitPoints == attackingPokemon.maxHitPoints && move.Type == ElementType.Flying)
        {
            return 1;
        }
        return base.AdjustSpeedPriorityOfMove(attackingPokemon, move);
    }
}