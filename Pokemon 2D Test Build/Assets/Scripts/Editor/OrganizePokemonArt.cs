using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;

public class OrganizePokemonArt : EditorWindow {

    int startingInteger = 001;
    int endingInteger = 001;
    bool includesShiny = true;
    [Tooltip("How many different Pokemon run from left to right")]
    int howManyPerLine = 9;

    TextureImporter assetImporter = new TextureImporter();
    string assetPath;

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
        //includesShiny = EditorGUILayout.Toggle("Shinies included in sprite Sheet?",true);
        //howManyPerLine = EditorGUILayout.IntField("How Many Per Line", howManyPerLine);

        if(GUILayout.Button("Organize Pokemon Selection"))
        {
            Debug.Log(Selection.objects);

            if(Selection.objects[0].GetType().Equals(typeof(Texture2D)))
            {
                OrganizeSelection();
            }
        }
    }

    void OrganizeSelection()
    {
        string newName = "";
        int currentVariation = 0;

        Texture2D texture2 = Selection.objects[0] as Texture2D;

        OnPreprocessTexture();
        OnPostprocessTexture(texture2);
        //OnPostprocessSprites(texture2, assetImporter);


        //foreach (Texture2D texturex in Selection.objects)
        //{

        //    newName = startingInteger.ToString("000");
        //    newName += "_";
        //    newName += Variant(currentVariation);
        //    currentVariation++;

        //    if(currentVariation >=4)
        //    {
        //        currentVariation = 0;
        //        startingInteger++;
        //    }


        //}
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


    private void OnPreprocessTexture()
    {
        Debug.Log("OnPreprocessTexture overwriting defaults");

        TextureImporter importer = assetImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.mipmapEnabled = true;
        importer.filterMode = FilterMode.Trilinear;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
    }

    public void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter importer = assetImporter as TextureImporter;
        if (importer.spriteImportMode != SpriteImportMode.Multiple)
        {
            Debug.Log("Sprite is not a multiple sprite");
            return;
        }

        Debug.Log("OnPostprocessTexture generating sprites");

        int minimumSpriteSize = 16;
        int extrudeSize = 0;
        Rect[] rects = InternalSpriteUtility.GenerateAutomaticSpriteRectangles(texture, minimumSpriteSize, extrudeSize);
        List<Rect> rectsList = new List<Rect>(rects);
        rectsList = SortRects(rectsList, texture.width);

        string filenameNoExtension = Path.GetFileNameWithoutExtension(assetPath);
        List<SpriteMetaData> metas = new List<SpriteMetaData>();
        int rectNum = 0;

        foreach (Rect rect in rectsList)
        {
            SpriteMetaData meta = new SpriteMetaData();
            meta.rect = rect;
            meta.name = filenameNoExtension + "_" + rectNum++;
            metas.Add(meta);
        }

        importer.spritesheet = metas.ToArray();
    }

    public void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
    {
        Debug.Log("OnPostprocessSprites sprites: " + sprites.Length);
    }

    private List<Rect> SortRects(List<Rect> rects, float textureWidth)
    {
        List<Rect> list = new List<Rect>();
        while (rects.Count > 0)
        {
            Rect rect = rects[rects.Count - 1];
            Rect sweepRect = new Rect(0f, rect.yMin, textureWidth, rect.height);
            List<Rect> list2 = this.RectSweep(rects, sweepRect);
            if (list2.Count <= 0)
            {
                list.AddRange(rects);
                break;
            }
            list.AddRange(list2);
        }
        return list;
    }

    private List<Rect> RectSweep(List<Rect> rects, Rect sweepRect)
    {
        List<Rect> result;
        if (rects == null || rects.Count == 0)
        {
            result = new List<Rect>();
        }
        else
        {
            List<Rect> list = new List<Rect>();
            foreach (Rect current in rects)
            {
                if (current.Overlaps(sweepRect))
                {
                    list.Add(current);
                }
            }
            foreach (Rect current2 in list)
            {
                rects.Remove(current2);
            }
            list.Sort((Rect a, Rect b) => a.x.CompareTo(b.x));
            result = list;
        }
        return result;
    }

}