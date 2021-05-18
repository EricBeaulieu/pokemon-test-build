using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPoint : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.PoisonPoint; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new PoisonPoint(); }
    public override string Description()
    {
        return "Contact with the Pokémon may poison the attacker.";
    }
    public override ConditionID ContactMoveMayCauseStatusEffect(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack)
    {
        if (currentAttack.PhysicalContact == true)
        {
            if (attackingPokemon.CheckStatusImmunities(ConditionID.Poison) == false)
            {
                if (Random.value < 0.3f)
                {
                    return ConditionID.Poison;
                }
            }
        }
        return base.ContactMoveMayCauseStatusEffect(defendingPokemon, attackingPokemon, currentAttack);
    }
}
