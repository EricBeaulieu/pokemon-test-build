using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anticipation : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Anticipation; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Anticipation(); }
    public override string Description()
    {
        return "The Pokémon can sense an opposing Pokémon's dangerous moves.";
    }
    public override bool ActivateAbilityUponEntry(Pokemon defendingPokemon, BattleUnit opposingTarget)
    {
        for (int i = 0; i < opposingTarget.pokemon.moves.Count; i++)
        {
            if(DamageModifiers.TypeChartEffectiveness(defendingPokemon.pokemonBase, opposingTarget.pokemon.moves[i].moveBase.Type) > 1)
            {
                return true;
            }
        }
        return base.ActivateAbilityUponEntry(defendingPokemon,opposingTarget);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName} shuttered!";
    }
}
