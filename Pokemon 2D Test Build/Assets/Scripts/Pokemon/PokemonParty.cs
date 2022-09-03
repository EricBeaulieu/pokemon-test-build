using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemonParty;
    List<Pokemon> _originalPos;

    public static int MAX_PARTY_POKEMON_SIZE { get; } = 6;

    void Awake()
    {
        int currentCount = 0;
        _originalPos = new List<Pokemon>();

        foreach (Pokemon pokemon in pokemonParty)
        {
            if(currentCount < MAX_PARTY_POKEMON_SIZE)
            {
                //Debug.Log($"{pokemon.currentName}", gameObject);
                pokemon.SetUpData();
                _originalPos.Add(pokemon);
            }
            else
            {
                Debug.LogWarning("This trainer has more pokemon in his party then it is allowed", gameObject);
                break;
            }
            currentCount++;
        }
        pokemonParty = new List<Pokemon>(_originalPos);

        ///This is just to set the players pokemon in his party ID
        if(GetComponent<PlayerController>() != null)
        {
            for (int i = 0; i < pokemonParty.Count; i++)
            {
                pokemonParty[i].Obtained(GetComponent<PlayerController>(),GameManager.instance.StandardPokeball);
            }
        }
    }

    public Pokemon GetFirstHealthyPokemon()
    {
        return pokemonParty.Where(x => x.currentHitPoints > 0).FirstOrDefault();
    }

    public List<Pokemon> CurrentPokemonList()
    {
        return pokemonParty;
    }

    public void AddCapturedPokemon(Pokemon capturedPokemon,PokeballItem currentPokeball)
    {
        _originalPos.Add(capturedPokemon);
    }

    public void AddGiftPokemon(Pokemon giftPokemon, PokeballItem pokeball = null)
    {
        if (pokeball == null)
        {
            pokeball = Resources.Load<PokeballItem>("Items/Pokeballs/Poke Ball");
        }

        giftPokemon.Obtained(GetComponent<PlayerController>(), pokeball);
        pokemonParty.Add(giftPokemon);
    }

    public void SwitchPokemonPositions(Pokemon currentPokemon, Pokemon newPokemon)
    {
        int currentIndex = pokemonParty.IndexOf(currentPokemon);
        int newIndex = pokemonParty.IndexOf(newPokemon);

        pokemonParty.RemoveAt(currentIndex);

        if (newIndex > currentIndex)
        {
            //If the index could have shifted due to the removal
            newIndex--;
        }

        pokemonParty.RemoveAt(newIndex);
        pokemonParty.Insert(newIndex, currentPokemon);

        pokemonParty.Insert(currentIndex, newPokemon);
    }

    public void SetOriginalPositions()
    {
        _originalPos = new List<Pokemon>(pokemonParty);
    }

    public void SetPositionstoBeforeBattle()
    {
        pokemonParty = _originalPos;
    }

    public void CleanUpPartyOrderOnStart(Pokemon startPokemon)
    {
        if (startPokemon != pokemonParty[0])
        {
            SwitchPokemonPositions(pokemonParty[0], startPokemon);
        }
    }

    public void HealAllPokemonInParty()
    {
        for (int i = 0; i < pokemonParty.Count; i++)
        {
            pokemonParty[i].FullyHeal();
        }
    }

    public int HealthyPokemonCount()
    {
        int currentCount = 0;

        for (int i = 0; i < pokemonParty.Count; i++)
        {
            if(pokemonParty[i].currentHitPoints> 0)
            {
                currentCount++;
            }
        }
        return currentCount;
    }

    public void LoadPlayerParty(List<Pokemon> loadedParty)
    {
        pokemonParty = loadedParty;
    }

    public Pokemon ContainsMove(string specificMove)
    {
        for (int i = 0; i < pokemonParty.Count; i++)
        {
            if (pokemonParty[i].moves.Exists(x => x.moveBase.MoveName == specificMove))
            {
                return pokemonParty[i];
            }
        }

        return null;
    }

    public bool PartyIsFull()
    {
        return (pokemonParty.Count >= MAX_PARTY_POKEMON_SIZE);
    }
}
