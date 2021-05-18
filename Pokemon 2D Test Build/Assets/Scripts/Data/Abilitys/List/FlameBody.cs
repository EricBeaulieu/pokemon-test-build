using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBody : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.FlameBody; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new FlameBody(); }
    public override string Description()
    {
        return "Contact with the Pokémon may burn the attacker.";
    }
    public override ConditionID ContactMoveMayCauseStatusEffect(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack)
    {
        if (currentAttack.PhysicalContact == true)
        {
            if (attackingPokemon.CheckStatusImmunities(ConditionID.Burn) == false)
            {
                if (Random.value < 0.3f)
                {
                    return ConditionID.Burn;
                }
            }
        }
        return base.ContactMoveMayCauseStatusEffect(defendingPokemon, attackingPokemon, currentAttack);
    }
}
