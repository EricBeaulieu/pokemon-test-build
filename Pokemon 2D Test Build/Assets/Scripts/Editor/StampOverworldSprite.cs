using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;
using System.Linq;

public class StampOverworldSprite : EditorWindow
{
    TextureImporter assetImporter = new TextureImporter();
    string assetPath;
    bool isPlayer = true;

    Vector2 standardSize = new Vector2(31, 31);
    int difVariations = 60;//70 total
    Vector2 startingSlicingPos = new Vector2(0, 124);
    Vector2 uniqueStartingSlicingPos = new Vector2(0,0);

    [MenuItem("Tools/Stamp Overworld Sprite")]
    public static void ShowWindow()
    {
        GetWindow(typeof(StampOverworldSprite));
    }

    void OnGUI()
    {
        GUILayout.Label("Stamp Overworld Sprite Art", EditorStyles.boldLabel);

        isPlayer = EditorGUILayout.Toggle("is Player: ", isPlayer);
        //ScriptableObject target = this;
        //SerializedObject sO = new SerializedObject(target);
        ////SerializedProperty intProperty = sO.FindProperty("differentGenderArt");
        ////EditorGUILayout.PropertyField(intProperty, true);
        //sO.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button("Organize Selection"))
        {
            if(Selection.objects[0] == null)
            {
                return;
            }

            if (isPlayer == false)
            {
                difVariations = 12;
                startingSlicingPos = new Vector2(startingSlicingPos.x, startingSlicingPos.y - standardSize.y);
            }

            if (Selection.objects[0].GetType().Equals(typeof(Texture2D)))
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    Texture2D texture = Selection.objects[i] as Texture2D;

                    ProcessTexture(texture);
                }
            }
        }
    }

    string Direction(int i)
    {
        switch (i)
        {
            case 0:
                return "Up";
            case 1:
                return "Down";
            case 2:
                return "Left";
            default:
                return "Right";
        }
    }

    string Variation(int i)
    {
        switch (i)
        {
            case 0:
                return "Idle";
            case 1:
                return "Walk_0";
            case 2:
                return "Walk_1";
            case 3:
                return "Run_0";
            case 4:
                return "Run_1";
            case 5:
                return "Run_2";
            case 6:
                return "Bike_0";
            case 7:
                return "Bike_1";
            case 8:
                return "Bike_2";
            case 9:
                return "Bike_3";
            case 10:
                return "Surf";
            case 11:
                return "Fish_0";
            case 12:
                return "Fish_1";
            case 13:
                return "Fish_2";
            default:
                return "Fish_3";
        }
    }

    string UniqueVariation(int i)
    {
        switch (i)
        {
            case 0:
                return "HM_0";
            case 1:
                return "HM_1";
            case 2:
                return "HM_2";
            case 3:
                return "HM_3";
            case 4:
                return "Item_0";
            case 5:
                return "Item_1";
            case 6:
                return "Left_BikeJump_0";
            case 7:
                return "Left_BikeJump_1";
            case 8:
                return "Right_BikeJump_0";
            default:
                return "Right_BikeJump_1";
        }
    }

    void ProcessTexture(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 16;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        var textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);
        textureSettings.spriteMeshType = SpriteMeshType.Tight;
        textureSettings.spriteExtrude = 0;

        importer.SetTextureSettings(textureSettings);

        Rect[] mainImage = InternalSpriteUtility.GenerateAutomaticSpriteRectangles(texture, 1, 0);
        List<Rect> rectsList = new List<Rect>(mainImage);
        rectsList = IndividualizeEachSlice(rectsList);

        string filenameNoExtension = Path.GetFileNameWithoutExtension(path);
        var metas = new List<SpriteMetaData>();

        int currentVariant = 0;

        foreach (Rect rect in rectsList)
        {
            var meta = new SpriteMetaData();
            meta.pivot = Vector2.zero;
            meta.alignment = (int)SpriteAlignment.Center;
            meta.rect = rect;
            string spriteName;

            if (currentVariant < difVariations)
            {
                spriteName = $"{Direction(currentVariant % 4)}_{Variation(Mathf.FloorToInt(currentVariant / 4))}";
            }
            else
            {
                spriteName = $"{UniqueVariation(currentVariant % difVariations)}";
            }

            currentVariant++;
            meta.name = spriteName;
            metas.Add(meta);
        }

        importer.spritesheet = metas.ToArray();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    List<Rect> IndividualizeEachSlice(List<Rect> slicedUpList)
    {
        List<Rect> list = new List<Rect>();

        int howManyPerColoum = 4;
        for (int i = 0; i < difVariations; i++)
        {
            int XPos = (int)startingSlicingPos.x + ((int)standardSize.x * (i / howManyPerColoum));
            int YPos = (int)startingSlicingPos.y - ((int)standardSize.y * ((i % howManyPerColoum)));

            list.Add(new Rect(XPos, YPos, standardSize.x, standardSize.y));
        }

        if (isPlayer == false)
        {
            return list;
        }

        int uniqueVariations = 10;
        for (int i = 0; i < uniqueVariations; i++)
        {
            int XPos = (int)uniqueStartingSlicingPos.x + ((int)standardSize.x * i);
            int YPos = (int)uniqueStartingSlicingPos.y;

            list.Add(new Rect(XPos, YPos, standardSize.x, standardSize.y));
        }

        return list;
    }
}
