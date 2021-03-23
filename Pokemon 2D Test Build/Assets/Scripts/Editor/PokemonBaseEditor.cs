using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(PokemonBase))]
public class PokemonBaseEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PokemonBase script = (PokemonBase)target;

        // draw checkbox for the bool
        script.DifferentGenderSprites = EditorGUILayout.Toggle("Difference In Gender Sprites", script.DifferentGenderSprites);

        if (script.DifferentGenderSprites) // if bool is true, show other fields
        {
            script.FemaleFrontRegularSprite = EditorGUILayout.ObjectField("Female Front Regular Sprite", script.FemaleFrontRegularSprite, typeof(Sprite), true) as Sprite;
            script.FemaleFrontIntroSprite = EditorGUILayout.ObjectField("Female Front Intro Sprite", script.FemaleFrontIntroSprite, typeof(Sprite), true) as Sprite;
            script.FemaleBackRegularSprite = EditorGUILayout.ObjectField("Female Back Regular Sprite", script.FemaleBackRegularSprite, typeof(Sprite), true) as Sprite;
            script.FemaleBackIntroSprite = EditorGUILayout.ObjectField("Female Back Intro Sprite", script.FemaleBackIntroSprite, typeof(Sprite), true) as Sprite;

            //Shiny
            script.FemaleShinyFrontRegularSprite = EditorGUILayout.ObjectField("Female Front Shiny Regular Sprite", script.FemaleShinyFrontRegularSprite, typeof(Sprite), true) as Sprite;
            script.FemaleShinyFrontIntroSprite = EditorGUILayout.ObjectField("Female Front Shiny Intro Sprite", script.FemaleShinyFrontIntroSprite, typeof(Sprite), true) as Sprite;
            script.FemaleShinyBackRegularSprite = EditorGUILayout.ObjectField("Female Back Shiny Regular Sprite", script.FemaleShinyBackRegularSprite, typeof(Sprite), true) as Sprite;
            script.FemaleShinyBackIntroSprite = EditorGUILayout.ObjectField("Female Back Shiny Intro Sprite", script.FemaleShinyBackIntroSprite, typeof(Sprite), true) as Sprite;
        }

        EditorUtility.SetDirty(script);

    }

    //private void OnDisable()
    //{
    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //}

    //private void OnEnable()
    //{
    //    AssetDatabase.Refresh();
    //}
}
