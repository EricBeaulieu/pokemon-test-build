using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemonParty;

    void Start()
    {
        foreach(Pokemon pokemon in pokemonParty)
        {
            pokemon.Initialization();
        }
    }

    public Pokemon GetFirstHealthyPokemon()
    {
        return pokemonParty.Where(x => x.currentHitPoints > 0).FirstOrDefault();
    }
}
