using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;
using System.Linq;

namespace PokeApi
{
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
            PokemonData pokemon;
            pokemon = APIHelper.GetPokemonData(pokedexNumber);
            pokemon.species = APIHelper.GetPokemonSpecies(pokedexNumber);

            string assetPath = $"Pokedex/{GetGenLocation(pokemon.id)}{pokemon.id.ToString("000")} {pokemon.name.UpperFirstChar()}";
            PokemonBase existingPokemon = Resources.Load<PokemonBase>(assetPath);

            if (existingPokemon != null)
            {
                Debug.Log($"Updated {pokemon.id.ToString("000")} {pokemon.name.UpperFirstChar()}");
                existingPokemon.Initialization(pokemon);
            }
            else
            {
                Debug.Log($"Created {pokemon.id.ToString("000")} {pokemon.name.UpperFirstChar()}");
                PokemonBase pokemonBase = ScriptableObject.CreateInstance<PokemonBase>();
                pokemonBase.Initialization(pokemon);
                AssetDatabase.CreateAsset(pokemonBase, $"Assets/Resources/{assetPath}.asset");
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
                default:
                    s = null;
                    break;
            }
            return s;
        }
    }
}

