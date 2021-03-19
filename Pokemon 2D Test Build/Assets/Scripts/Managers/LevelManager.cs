using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemon;

    public Pokemon WildPokemon()
    {
        Pokemon temp = wildPokemon[Random.Range(0, wildPokemon.Count)];
        temp.Initialization();
        return temp;
    }
}
