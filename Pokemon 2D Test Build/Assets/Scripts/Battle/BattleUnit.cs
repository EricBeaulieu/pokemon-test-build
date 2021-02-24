using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{

    //Testing
    [SerializeField]
    PokemonBase _pBase;
    [SerializeField]
    int _level;
    //

    [SerializeField]
    Image _pokemonSprite;
    [SerializeField]
    bool _isPlayersPokemon;

    public Pokemon pokemon {get;set;}

    public void Setup()
    {
        pokemon = new Pokemon(_pBase, _level);

        if(_isPlayersPokemon)
        {
            _pokemonSprite.sprite = pokemon.pokemonBase.GetBackSprite(false)[0];
        }
        else
        {
            _pokemonSprite.sprite = pokemon.pokemonBase.GetFrontSprite(false)[0];
        }
    }
}
