using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFetch : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.BallFetch; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new BallFetch(); }
    public override string Description()
    {
        return "If the Pokémon is not holding an item, it will fetch the Poké Ball from the first failed throw of the battle.";
    }
    public override void FetchPokeBallFirstFailedThrow(PokeballItem pokeballItem, Pokemon defendingPokemon)
    {
        if(defendingPokemon.GetCurrentItem == null)
        {
            defendingPokemon.GivePokemonItemToHold(pokeballItem);
        }
    }
}
