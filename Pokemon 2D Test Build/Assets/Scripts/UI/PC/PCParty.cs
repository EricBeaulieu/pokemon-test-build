using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCParty : MonoBehaviour
{
    [SerializeField] PCPartyPokemon[] partyPokemon;
    public static bool isOn { get; private set; } = false;

    const int Y_ON_SCREEN = 5;
    const int Y_OFF_SCREEN = 455;
    const float BOX_SPEED_ANIMATION = 0.25f;

    bool changingPokemon = false;

    public IEnumerator EnableParty()
    {
        Vector3 endPosition = transform.localPosition;
        if (isOn == true)
        {
            endPosition.y = Y_OFF_SCREEN;
        }
        else
        {
            endPosition.y = Y_ON_SCREEN;
        }

        isOn = !isOn;
        yield return GlobalTools.SmoothTransitionToPositionUsingLocalPosition(transform, endPosition, BOX_SPEED_ANIMATION);
    }

    public GameObject ReturnFirstSelection()
    {
        return partyPokemon[0].gameObject;
    }

    public void SetupData()
    {
        List<Pokemon> playerParty = GameManager.instance.GetPlayerController.pokemonParty.CurrentPokemonList();
        changingPokemon = true;// this is to prevent PartyPokemonChanged() from running at the start checking each pokemon and cycling them through
        for (int i = 0; i < partyPokemon.Length; i++)
        {
            if(i < playerParty.Count)
            {
                partyPokemon[i].DepositPokemon(playerParty[i]);
                continue;
            }
            partyPokemon[i].DepositPokemon(null);
        }
        changingPokemon = false;
    }

    public void PartyPokemonChanged()
    {
        if(changingPokemon == true)
        {
            return;
        }
        changingPokemon = true;
        for (int i = partyPokemon.Length; i >= 0 ; i--)
        {
            if(i+1 < partyPokemon.Length)
            {
                if(partyPokemon[i].currentPokemon != null)
                {
                    continue;
                }
                Debug.Log($"pos {i}", partyPokemon[i].gameObject);
                partyPokemon[i].DepositPokemon(partyPokemon[i + 1].WithdrawPokemon());
                if(i + 1 < partyPokemon.Length)
                {
                    for (int k = i; k < partyPokemon.Length; k++)
                    {
                        if (partyPokemon[k].currentPokemon != null || k >= partyPokemon.Length-1)
                        {
                            continue;
                        }
                        partyPokemon[k].DepositPokemon(partyPokemon[k + 1].WithdrawPokemon());
                    }
                }
            }
        }
        changingPokemon = false;
    }

    public List<Pokemon> PartyPokemon()
    {
        List<Pokemon> pokemon = new List<Pokemon>();

        foreach(PCPartyPokemon pCPartyPokemon in partyPokemon)
        {
            if(pCPartyPokemon.currentPokemon != null)
            {
                pokemon.Add(pCPartyPokemon.currentPokemon);
            }
        }
        return pokemon;
    }
    
    public int CurrentPartyCount()
    {
        int currentCount = 0;
        foreach (PCPartyPokemon pCPartyPokemon in partyPokemon)
        {
            if (pCPartyPokemon.currentPokemon != null)
            {
                currentCount++;
            }
        }
        return currentCount;
    }
}
