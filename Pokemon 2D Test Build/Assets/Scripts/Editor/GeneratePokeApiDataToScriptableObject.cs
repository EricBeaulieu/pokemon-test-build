using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;
using System.Linq;

namespace PokeApi
{
    /// <summary>
    /// For Unown, it was removed as his API calls are weird, so it will generate just a default variation
    /// </summary>
    public class GeneratePokeApiDataToScriptableObject : EditorWindow
    {
        int startingInteger = 001;
        int endingInteger = 000;
        static bool callDebug = true;
        static bool createdOrUpdateDebug = true;

        const int MIN_POKEMON_NUMBER = 1;
        const int MAX_POKEMON_NUMBER = 1008;

        [MenuItem("Tools/GeneratePokemon")]
        public static void ShowWindow()
        {
            GetWindow(typeof(GeneratePokeApiDataToScriptableObject));
        }

        void OnGUI()
        {
            GUILayout.Label("Organize Pokemon Art Name and Numbers", EditorStyles.boldLabel);
            startingInteger = EditorGUILayout.IntField("Starting Number: ", startingInteger);
            endingInteger = EditorGUILayout.IntField("Ending Number: ", endingInteger);
            callDebug = EditorGUILayout.Toggle("Call Debug: ", callDebug);
            createdOrUpdateDebug = EditorGUILayout.Toggle("Created/Updated Debug: ", createdOrUpdateDebug);

            if (GUILayout.Button("Generate Pokemon"))
            {
                if(startingInteger < MIN_POKEMON_NUMBER || startingInteger > MAX_POKEMON_NUMBER)
                {
                    return;
                }

                if (endingInteger < startingInteger)
                {
                    CreateScriptableObjectPokemonBase(startingInteger);
                }
                else
                {
                    for (int i = startingInteger; i < endingInteger + 1; i++)
                    {
                        if(i > MAX_POKEMON_NUMBER)
                        {
                            return;
                        }

                        CreateScriptableObjectPokemonBase(i);
                    }
                }
            }
        }

        static void CreateScriptableObjectPokemonBase(int pokedexNumber)
        {
            List<string> pokemonName = new List<string>();
            string originalName = PokemonNameList.GetPokeDexName(pokedexNumber);
            pokemonName.Add(originalName);

            if (PokemonNameList.PokemonDoesntHaveAnOriginalFormName(pokedexNumber) == true)
            {
                pokemonName.Clear();
            }

            string[] alternativeForms = PokemonNameList.PokemonAlternativeFormNames(pokedexNumber);

            if(alternativeForms != null)
            {
                for (int i = 0; i < alternativeForms.Length; i++)
                {
                    pokemonName.Add($"{originalName}#{alternativeForms[i]}");
                }
            }

            foreach (string tempPokemonName in pokemonName)
            {
                string currentPokemonName = tempPokemonName.Replace('#', '-');
                PokemonData pokemon;
                if (callDebug == true)
                {
                    Debug.Log($"Calling {currentPokemonName}");
                }
                
                if(PokemonNameList.specialCaseNames(pokedexNumber) == true)
                {
                    pokemon = APIHelper.GetPokemonData(pokedexNumber);
                    pokemon = APIHelper.GetPokemonData(currentPokemonName.Replace(originalName, pokemon.name.UpperFirstChar()));
                }
                else
                {
                    pokemon = APIHelper.GetPokemonData(currentPokemonName);
                }
                pokemon.species = APIHelper.GetPokemonSpecies(pokedexNumber);

                string assetPath = $"Pokedex/{GetGenLocation(pokedexNumber)}{pokedexNumber.ToString("000")} {currentPokemonName}";

                if(currentPokemonName.Contains("!"))//Unown
                {
                    assetPath.Replace("!", "QuestionMark");
                }

                PokemonBase existingPokemon = Resources.Load<PokemonBase>(assetPath);
                string formName = null;

                if(tempPokemonName.Contains('#'))
                {
                    formName = tempPokemonName.Split('#')[1];
                    //Debug.Log($"Form name provided is {formName}");
                }

                if (existingPokemon != null)
                {
                    if(createdOrUpdateDebug == true)
                    {
                        Debug.Log($"Updated {pokedexNumber.ToString("000")} {currentPokemonName}");
                    }
                    existingPokemon.Initialization(pokemon, pokedexNumber, formName);
                    EditorUtility.SetDirty(existingPokemon);
                }
                else
                {
                    if (createdOrUpdateDebug == true)
                    {
                        Debug.Log($"Created {pokedexNumber.ToString("000")} {currentPokemonName}");
                    }
                    PokemonBase pokemonBase = ScriptableObject.CreateInstance<PokemonBase>();
                    pokemonBase.Initialization(pokemon, pokedexNumber, formName);
                    AssetDatabase.CreateAsset(pokemonBase, $"Assets/Resources/{assetPath}.asset");
                }
                AssetDatabase.SaveAssets();
            }
        }

        static string GetGenLocation(int pokedexNumber)
        {
            string s;
            switch (pokedexNumber)
            {
                case int n when (n <= 151):
                    s = "Gen1/";
                    break;
                case int n when (n >= 152 && n <= 251):
                    s = "Gen2/";
                    break;
                case int n when (n >= 252 && n <= 386):
                    s = "Gen3/";
                    break;
                case int n when (n >= 387 && n <= 493):
                    s = "Gen4/";
                    break;
                case int n when (n >= 494 && n <= 649):
                    s = "Gen5/";
                    break;
                case int n when (n >= 650 && n <= 721):
                    s = "Gen6/";
                    break;
                case int n when (n >= 722 && n <= 809):
                    s = "Gen7/";
                    break;
                case int n when (n >= 810 && n <= 905):
                    s = "Gen8/";
                    break;
                case int n when (n >= 906 && n <= 1008):
                    s = "Gen9/";
                    break;
                default:
                    s = null;
                    break;
            }
            return s;
        }
    }
}

