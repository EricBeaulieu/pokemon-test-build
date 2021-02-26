using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PokemonBase))]
public class PokemonBaseEditor : Editor
{

    void OnInspectorGUI()
    {
        //var pokemonBase = target as PokemonBase;

        //pokemonBase.hasGender = GUILayout.Toggle(pokemonBase.hasGender,"Poo");

        //Debug.Log(pokemonBase.hasGender);
        //if (pokemonBase.hasGender)
        //    pokemonBase. = EditorGUILayout.Toggle()

    }

}
