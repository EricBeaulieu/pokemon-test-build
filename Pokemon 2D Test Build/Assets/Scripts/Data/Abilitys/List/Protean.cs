using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protean : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Protean; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Protean(); }
    public override string Description()
    {
        return "Changes the Pokémon's type to the type of the move it's about to use.";
    }
    public override ElementType ChangePokemonToCurrentAttackType(BattleUnit sourceUnit, MoveBase currentAttack)
    {
        if (currentAttack.MoveName == "Struggle")
        {
            return ElementType.NA;
        }

        if (sourceUnit.lastMoveUsed == null)
        {
            return currentAttack.Type;
        }

        if (sourceUnit.pokemon.IsType(currentAttack.Type) == true)
        {
            return ElementType.NA;
        }

        return currentAttack.Type;
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s transformed into {pokemon.pokemonType1} type!";
    }
}