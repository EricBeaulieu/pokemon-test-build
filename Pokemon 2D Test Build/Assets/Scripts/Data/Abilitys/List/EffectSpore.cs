using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpore : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.EffectSpore; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new EffectSpore(); }
    public override string Description()
    {
        return "Contact with the Pokémon may inflict poison, sleep, or paralysis on its attacker.";
    }
    public override ConditionID ContactMoveMayCauseStatusEffect(Pokemon defendingPokemon, Pokemon attackingPokemon, MoveBase currentAttack)
    {
        if (currentAttack.PhysicalContact == true)
        {
            float rnd = Random.Range(0, 100) / 100;
            ConditionID rndCondition = base.ContactMoveMayCauseStatusEffect(defendingPokemon, attackingPokemon, currentAttack);

            if (rnd <= 0.09f)
            {
                rndCondition = ConditionID.Poison;
            }
            else if (rnd > 0.09f && rnd < 0.2f)
            {
                rndCondition = ConditionID.Paralyzed;
            }
            else if (rnd >= 0.2f && rnd <= 0.3f)
            {
                rndCondition = ConditionID.Sleep;
            }
            else
            {
                return rndCondition;
            }


            if (attackingPokemon.CheckStatusImmunities(rndCondition) == false)
            {
                return rndCondition;
            }
        }
        return base.ContactMoveMayCauseStatusEffect(defendingPokemon, attackingPokemon, currentAttack);
    }
}
