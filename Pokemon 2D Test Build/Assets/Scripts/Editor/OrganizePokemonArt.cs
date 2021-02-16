using UnityEditor;
using UnityEngine;

public class OrganizePokemonArt : EditorWindow {

    int startingInteger = 001;
    int endingInteger = 001;
    bool includesShiny = true;
    [Tooltip("How many different Pokemon run from left to right")]
    int howManyPerLine = 9;

    [MenuItem("Tools/OrganizeNumbersForArt")]
    public static void ShowWindow()
    {
        GetWindow(typeof(OrganizePokemonArt));
    }

    private void OnGUI()
    {
        GUILayout.Label("Organize Pokemon Numbers", EditorStyles.boldLabel);
        startingInteger = EditorGUILayout.IntField("Starting Number: ", startingInteger);
        endingInteger = EditorGUILayout.IntField("Ending Number: ", endingInteger);
        includesShiny = EditorGUILayout.Toggle("Shinies included in sprite Sheet?",true);
        howManyPerLine = EditorGUILayout.IntField("How Many Per Line", howManyPerLine);

        if(GUILayout.Button("Organize Pokemon Selection"))
        {
            OrganizeSelection();
        }
    }

    void OrganizeSelection()
    {
        string newName = "";
        int currentVariation = 0;

        foreach(Sprite sprite in Selection.objects)
        {
            newName = startingInteger.ToString("000");
            newName += "_";
            newName += Variant(currentVariation);
            currentVariation++;

            if(currentVariation >=4)
            {
                currentVariation = 0;
                startingInteger++;
            }

            Debug.Log(AssetDatabase.GetAssetPath(sprite));

            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(sprite), newName);
        }
    }

    string Variant(int i)
    {
        switch (i)
        {
            case 0:
                return "FrontA";
            case 1:
                return "FrontB";
            case 2:
                return "BackA";
            case 3:
                return "BackB";
            default:
                Debug.Log("Broken");
                break;
        }
        return "Broken";
    }
}