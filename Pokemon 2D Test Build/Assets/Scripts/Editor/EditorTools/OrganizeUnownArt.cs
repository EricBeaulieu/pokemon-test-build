using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;
using System.Linq;

public class OrganizeUnownArt : EditorWindow
{
    //How many different Pokemon run from left to right
    int howManyPerLine = 10;

    TextureImporter assetImporter = new TextureImporter();
    string assetPath;

    /*All original art comes from https://www.spriters-resource.com/ds_dsi/pokemonplatinum/
    In this game the art is a modified version, it is converted into a PNG to give it a transparent background.
    In the originals there is a flat colour amongst all them and some issues with the original slicing. (weedles a perfect example)
    This was all done through photoshop, the art provided by me is a modified version
    */

    Vector2 pokemonArtMainSize = new Vector2(162, 195);
    //each slice that houses all of the sprites

    Vector2 pokemonArtStandardSize = new Vector2(80, 80);

    Vector2 pokemonFrontAPos = new Vector2(1, 82);
    Vector2 pokemonBackAPos = new Vector2(82, 82);
    Vector2 pokemonShinyFrontAPos = new Vector2(1, 1);
    Vector2 pokemonShinyBackAPos = new Vector2(82, 1);

    Vector2 pokemonArtSpriteSize = new Vector2(32, 32);
    //size of the sprite used within the party screen

    Vector2 pokemonSpriteAPos = new Vector2(97, 163);
    Vector2 pokemonSpriteBPos = new Vector2(130, 163);
    //positions of the 2 alternating sprites

    Vector2 pokemonArtFootprintSize = new Vector2(16, 16);
    //size of the pokemons footprint size

    [MenuItem("Tools/OrganizeUnownForArt")]
    public static void ShowWindow()
    {
        GetWindow(typeof(OrganizeUnownArt));
    }

    void OnGUI()
    {
        GUILayout.Label("Organize Pokemon Art Name and Numbers", EditorStyles.boldLabel);

        ScriptableObject target = this;
        SerializedObject sO = new SerializedObject(target);

        sO.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button("Organize Pokemon Selection"))
        {
            if (Selection.objects[0].GetType().Equals(typeof(Texture2D)))
            {
                Texture2D texture2 = Selection.objects[0] as Texture2D;
                ProcessTexture(texture2);
            }
        }
    }

    /// <summary>
    /// this helps label the in battle sprites accordingly
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    string Variant(int i)
    {
        switch (i)
        {
            case 0:
                return "FrontA";
            case 1:
                return "BackA";
            default:
                Debug.Log("Broken");
                break;
        }
        return "Broken";
    }

    void ProcessTexture(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        importer.spritePivot = Vector2.up;
        importer.maxTextureSize = 16384;
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        var textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);
        textureSettings.spriteMeshType = SpriteMeshType.Tight;
        textureSettings.spriteExtrude = 0;

        importer.SetTextureSettings(textureSettings);

        //updated the texture but using old values of the texture. need to update it
        //required to force the updated setting on the texture then continue. rect list is taking in old values causing issues

        Rect[] mainImage = InternalSpriteUtility.GenerateAutomaticSpriteRectangles(texture, 1, 0);
        var rectsList = new List<Rect>(mainImage);
        rectsList = SliceUpPokemonToOwnRects(rectsList, texture.width, texture.height);
        rectsList = IndividualizeEachPokemonSlice(rectsList);

        string filenameNoExtension = Path.GetFileNameWithoutExtension(path);
        var metas = new List<SpriteMetaData>();

        int pokedexNumber = 0;
        int currentVariant = 0;

        foreach (Rect rect in rectsList)
        {
            var meta = new SpriteMetaData();
            meta.pivot = Vector2.zero;
            meta.alignment = (int)SpriteAlignment.Center;
            meta.rect = rect;
            string spriteName = $"201_Unown_{PokemonNameList.UnownForm[pokedexNumber]}_";

            if (currentVariant > 1 && currentVariant < 4)
            {
                spriteName += "Shiny_";
            }

            if (currentVariant < 4)
            {
                spriteName += Variant(currentVariant % 2);
            }
            else if (currentVariant == 4)
            {
                spriteName += "SpriteA";
            }
            else if (currentVariant == 5)
            {
                spriteName += "SpriteB";
            }


            currentVariant++;
            if (currentVariant >= 6)
            {
                pokedexNumber++;
                currentVariant = 0;
            }

            meta.name = spriteName;
            metas.Add(meta);
        }

        importer.spritesheet = metas.ToArray();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    List<Rect> SliceUpPokemonToOwnRects(List<Rect> rects, int textureWidth, int textureHeight)
    {
        List<Rect> list = new List<Rect>();

        for (int i = 0; i < (PokemonNameList.UnownForm.Length); i++)
        {
            int XPos = ((int)pokemonArtMainSize.x * (i % howManyPerLine));
            int YPos = textureHeight - ((int)pokemonArtMainSize.y * ((i / howManyPerLine) + 1));

            list.Add(new Rect(XPos, YPos, pokemonArtMainSize.x, pokemonArtMainSize.y));
        }

        return list;
    }

    List<Rect> IndividualizeEachPokemonSlice(List<Rect> slicedUpList)
    {
        List<Rect> list = new List<Rect>();

        for (int i = 0; i < slicedUpList.Count; i++)
        {
            //Regular

            int XPos = (int)slicedUpList[i].x + (int)pokemonFrontAPos.x;
            int YPos = (int)slicedUpList[i].y + (int)pokemonFrontAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));


            XPos = (int)slicedUpList[i].x + (int)pokemonBackAPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonBackAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));


            //Shiny

            XPos = (int)slicedUpList[i].x + (int)pokemonShinyFrontAPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonShinyFrontAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));


            XPos = (int)slicedUpList[i].x + (int)pokemonShinyBackAPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonShinyBackAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            //Sprite

            XPos = (int)slicedUpList[i].x + (int)pokemonSpriteAPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonSpriteAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtSpriteSize.x, pokemonArtSpriteSize.y));

            XPos = (int)slicedUpList[i].x + (int)pokemonSpriteBPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonSpriteBPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtSpriteSize.x, pokemonArtSpriteSize.y));
        }
        return list;
    }
}
