using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.ColorChange; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new ColorChange(); }
    public override string Description()
    {
        return "The Pokémon's type becomes the type of the move used on it.";
    }
    public override ElementType ChangePokemonToCurrentType(Pokemon defendingPokemon, MoveBase currentAttack)
    {
        if(currentAttack.MoveName == "Struggle")
        {
            return ElementType.NA;
        }

        if(defendingPokemon.IsType(currentAttack.Type) == true)
        {
            return ElementType.NA;
        }

        return currentAttack.Type;
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName}'s {Name} made it {pokemon.pokemonType1} type!";
    }
}
