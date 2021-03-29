using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemonParty;

    const int MAX_PARTY_POKEMON_SIZE = 6;

    void Start()
    {
        int currentCount = 0;
        List<Pokemon> copyOfParty = new List<Pokemon>();

        foreach (Pokemon pokemon in pokemonParty)
        {
            if(currentCount < MAX_PARTY_POKEMON_SIZE)
            {
                copyOfParty.Add(new Pokemon(pokemon.pokemonBase, pokemon.currentLevel));
            }
            else
            {
                Debug.LogWarning("This trainer has more pokemon in his party then it is allowed", gameObject);
                break;
            }
            currentCount++;
        }
        pokemonParty = new List<Pokemon>(copyOfParty);
    }

    public Pokemon GetFirstHealthyPokemon()
    {
        return pokemonParty.Where(x => x.currentHitPoints > 0).FirstOrDefault();
    }

    public List<Pokemon> CurrentPokemonList()
    {
        return pokemonParty;
    }

    public bool AddCapturedPokemon(Pokemon capturedPokemon)
    {
        if(pokemonParty.Count < MAX_PARTY_POKEMON_SIZE)
        {
            pokemonParty.Add(capturedPokemon);
            return true;
        }
        else
        {
            //Send to PC
            return false;
        }
    }
}
