using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPartyPokemon : PCPokemon
{
    [SerializeField] PCParty partyUI;

    public override void DepositPokemon(Pokemon newPokemon)
    {
        base.DepositPokemon(newPokemon);
        partyUI.PartyPokemonChanged();
    }

    public override Pokemon WithdrawPokemon()
    {
        Pokemon tempPokemon = base.WithdrawPokemon();
        partyUI.PartyPokemonChanged();
        return tempPokemon;
    }
}
