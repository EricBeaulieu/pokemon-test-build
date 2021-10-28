using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frisk : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Frisk; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Frisk(); }
    public override string Description()
    {
        return "When it enters a battle, the Pokémon can check an opposing Pokémon's held item.";
    }
    string abilityMessage;
    public override bool ActivateAbilityUponEntry(BattleUnit defendingPokemon, BattleUnit opposingTarget)
    {
        if (opposingTarget.pokemon.GetCurrentItem == null)
        {
            return false;
        }

        abilityMessage = $"The {defendingPokemon.pokemon.currentName} frisked {opposingTarget.pokemon.currentName} and found it's {opposingTarget.pokemon.GetCurrentItem.ItemName}!";
        return true;
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{abilityMessage}";
    }
}
