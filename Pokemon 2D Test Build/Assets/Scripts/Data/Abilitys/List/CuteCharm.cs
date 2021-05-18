using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuteCharm : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.CuteCharm; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new CuteCharm(); }
    public override string Description()
    {
        return "Contact with the Pokémon may cause infatuation.";
    }
    public override ConditionID ContactMoveMayCauseStatusEffect(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack)
    {
        if (currentAttack.PhysicalContact == true)
        {
            if (attackingPokemon.CheckStatusImmunities(ConditionID.Infatuation) == false)
            {
                if (BattleSystem.CheckIfTargetCanBeInflatuated(defendingPokemon, attackingPokemon) == true)
                {
                    if (Random.value < 0.3f)
                    {
                        return ConditionID.Infatuation;
                    }
                }
            }
        }
        return base.ContactMoveMayCauseStatusEffect(defendingPokemon, attackingPokemon, currentAttack);
    }
}
