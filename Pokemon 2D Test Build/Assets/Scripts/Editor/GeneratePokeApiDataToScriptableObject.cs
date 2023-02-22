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

            if (GUILayout.Button("Generate Pokemon"))
            {
                if (endingInteger < startingInteger)
                {
                    CreateScriptableObjectPokemonBase(startingInteger);
                }
                else
                {
                    for (int i = startingInteger; i < endingInteger + 1; i++)
                    {
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
                    pokemonName.Add($"{originalName}-{alternativeForms[i]}");
                }
            }

            foreach (string tempPokemonName in pokemonName)
            {
                string currentPokemonName = tempPokemonName;
                PokemonData pokemon;
                Debug.Log($"Calling {currentPokemonName}");
                
                if(specialCaseNames(pokedexNumber) == true)
                {
                    pokemon = APIHelper.GetPokemonData(pokedexNumber);
                }
                else
                {
                    pokemon = APIHelper.GetPokemonData(currentPokemonName.ToLower());
                }
                pokemon.species = APIHelper.GetPokemonSpecies(pokedexNumber);

                if(specialCaseNames(pokedexNumber) == true)
                {
                    Debug.Log($"Before {currentPokemonName}");
                    currentPokemonName = currentPokemonName.Replace(originalName, pokemon.name.UpperFirstChar());
                    Debug.Log($"After {currentPokemonName}");
                }

                string assetPath = $"Pokedex/{GetGenLocation(pokedexNumber)}{pokedexNumber.ToString("000")} {currentPokemonName}";

                if(currentPokemonName.Contains("!"))//Unown
                {
                    assetPath.Replace("!", "QuestionMark");
                }

                PokemonBase existingPokemon = Resources.Load<PokemonBase>(assetPath);

                if (existingPokemon != null)
                {
                    Debug.Log($"Updated {pokedexNumber.ToString("000")} {currentPokemonName}");
                    existingPokemon.Initialization(pokemon, pokedexNumber);
                    EditorUtility.SetDirty(existingPokemon);
                }
                else
                {
                    Debug.Log($"Created {pokedexNumber.ToString("000")} {currentPokemonName}");
                    PokemonBase pokemonBase = ScriptableObject.CreateInstance<PokemonBase>();
                    pokemonBase.Initialization(pokemon, pokedexNumber);
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

        static bool specialCaseNames(int pokedexNumber)
        {
            if(pokedexNumber == 29 || pokedexNumber == 32 || pokedexNumber == 83 || pokedexNumber == 122 || pokedexNumber == 439)
            {
                return true;
            }
            return false;
        }
    }
}

