using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Static; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Static(); }
    public override string Description()
    {
        return "The Pokémon is charged with static electricity, so contact with it may cause paralysis.";
    }
    public override ConditionID ContactMoveMayCauseStatusEffect(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack)
    {
        if (currentAttack.PhysicalContact == true)
        {
            if (attackingPokemon.CheckStatusImmunities(ConditionID.Paralyzed) == false)
            {
                if (Random.value < 0.3f)
                {
                    return ConditionID.Paralyzed;
                }
            }
        }
        return base.ContactMoveMayCauseStatusEffect(defendingPokemon, attackingPokemon, currentAttack);
    }
}
