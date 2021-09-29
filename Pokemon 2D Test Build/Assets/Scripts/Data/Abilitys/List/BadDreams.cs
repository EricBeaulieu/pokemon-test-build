using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDreams : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.BadDreams; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new BadDreams(); }
    public override string Description()
    {
        return "Reduces the HP of sleeping opposing Pokémon.";
    }
    public override bool ApplyEffectsToOpposingPokemonOnTurnEnd(BattleUnit opposingTarget)
    {
        if(opposingTarget.pokemon.currentHitPoints > 0 && opposingTarget.pokemon.status?.Id == ConditionID.Sleep)
        {
            int damage = opposingTarget.pokemon.maxHitPoints / 8;

            if (damage <= 0)
            {
                damage = 1;
            }

            opposingTarget.pokemon.UpdateHPDamage(damage);
            opposingTarget.pokemon.statusChanges.Enqueue($"{opposingTarget.pokemon.currentName} is tormented");
            return true;
        }
        return base.ApplyEffectsToOpposingPokemonOnTurnEnd(opposingTarget);
    }
}
