using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;
using System.Linq;

public class OrganizePokemonArt : EditorWindow
{
    int startingInteger = 001;
    int endingInteger = 001;
    //How many different Pokemon run from left to right
    int howManyPerLine = 10;
    List<string> pokemonNames = new List<string>();
    List<int> differentGenderArt = new List<int>();

    TextureImporter assetImporter = new TextureImporter();
    string assetPath;

    /*All original art comes from https://www.spriters-resource.com/ds_dsi/pokemonplatinum/
    In this game the art is a modified version, it is converted into a PNG to give it a transparent background.
    In the originals there is a flat colour amongst all them and some issues with the original slicing. (weedles a perfect example)
    This was all done through photoshop, the art provided by me is a modified version
    */

    Vector2 pokemonArtMainSize = new Vector2(324, 195);
    //each slice that houses all of the sprites

    Vector2 startingSlicingPos = new Vector2(0, 1);
    //this is due to an offset

    Vector2 pokemonArtStandardSize = new Vector2(80, 80);
    //size of each in game battle sprite

    Vector2 pokemonFrontAPos = new Vector2(1, 82);
    Vector2 pokemonFrontBPos = new Vector2(82, 82);
    Vector2 pokemonBackAPos = new Vector2(163, 82);
    Vector2 pokemonBackBPos = new Vector2(244, 82);
    Vector2 pokemonShinyFrontAPos = new Vector2(1, 1);
    Vector2 pokemonShinyFrontBPos = new Vector2(82, 1);
    Vector2 pokemonShinyBackAPos = new Vector2(163, 1);
    Vector2 pokemonShinyBackBPos = new Vector2(244, 1);
    //position of each in game battle sprite in according to its housing size

    Vector2 pokemonArtSpriteSize = new Vector2(32, 32);
    //size of the sprite used within the party screen

    Vector2 pokemonSpriteAPos = new Vector2(259, 163);
    Vector2 pokemonSpriteBPos = new Vector2(292, 163);
    //positions of the 2 alternating sprites

    Vector2 pokemonArtFootprintSize = new Vector2(16, 16);
    //size of the pokemons footprint size

    Vector2 pokemonFootprintPos = new Vector2(242, 163);
    //positition of the foot print

    [MenuItem("Tools/OrganizeNumbersForArt")]
    public static void ShowWindow()
    {
        GetWindow(typeof(OrganizePokemonArt));
    }

    void OnGUI()
    {
        GUILayout.Label("Organize Pokemon Art Name and Numbers", EditorStyles.boldLabel);
        startingInteger = EditorGUILayout.IntField("Starting Number: ", startingInteger);
        endingInteger = EditorGUILayout.IntField("Ending Number: ", endingInteger);

        ScriptableObject target = this;
        SerializedObject sO = new SerializedObject(target);

        sO.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button("Organize Pokemon Selection"))
        {
            if (Selection.objects[0].GetType().Equals(typeof(Texture2D)))
            {
                if (endingInteger < startingInteger)
                {
                    return;
                }

                pokemonNames.Clear();
                differentGenderArt.Clear();

                for (int i = startingInteger; i < endingInteger + 1; i++)
                {
                    pokemonNames.Add(PokemonNameList.GetPokeDexName(i));
                    string[] differentFormNames = PokemonAlternativeFormNamesForArt(i);

                    if (differentFormNames != null)
                    {
                        //if(PokemonNameList.GenderExclusive(i) == false)
                        differentGenderArt.Add(i);
                    }
                }

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

        int pokedexNumber = startingInteger;
        int currentVariant = 0;
        bool skipArtAddition = false;
        string[] differentForms = null;
        int differentFormVariation = 0;
        int differentFormVariationsTotal = 0;
        int totalArtSlices = 0;

        foreach (Rect rect in rectsList)
        {
            var meta = new SpriteMetaData();
            meta.pivot = Vector2.zero;
            meta.alignment = (int)SpriteAlignment.Center;
            meta.rect = rect;
            Debug.Log(pokemonNames[pokedexNumber - startingInteger]);
            string spriteName = $"{pokedexNumber.ToString("000")}_{pokemonNames[pokedexNumber - startingInteger]}_";
            skipArtAddition = false;

            if (currentVariant == 0)
            {
                differentForms = PokemonAlternativeFormNamesForArt(pokedexNumber);
                if (differentForms != null)
                {
                    differentFormVariationsTotal = 11;
                    totalArtSlices = 11 * differentForms.Length;
                    //if (PokemonNameList.GenderExclusive(pokedexNumber) == true)
                    //{
                    //    differentFormVariationsTotal = 8;
                    //    totalArtSlices = (differentFormVariationsTotal * differentForms.Length) + 3;
                    //}
                    //else
                    //{
                    //    differentFormVariationsTotal = 11;
                    //    totalArtSlices = 11 * differentForms.Length;
                    //}
                }
                else
                {
                    differentFormVariationsTotal = 11;
                    totalArtSlices = 11;
                }
            }

            if ((currentVariant % differentFormVariationsTotal) > 3 && (currentVariant % differentFormVariationsTotal) < 8)
            {
                spriteName += "Shiny_";
            }

            if (differentForms != null)
            {
                differentFormVariation = Mathf.FloorToInt(currentVariant / differentFormVariationsTotal);
                if (differentFormVariation < differentForms.Length)
                {
                    if (string.IsNullOrEmpty(differentForms[differentFormVariation]) == false)
                    {
                        spriteName += $"{differentForms[differentFormVariation]}_";
                    }
                }

                if(PokemonNameList.GenderExclusive(pokedexNumber) == true)
                {
                    if ((currentVariant % differentFormVariationsTotal) < 8)
                    {
                        spriteName += Variant(currentVariant % 4);
                    }
                    else if ((currentVariant % differentFormVariationsTotal) == 8)
                    {
                        if (differentFormVariation != 1)
                        {
                            skipArtAddition = true;
                        }
                        spriteName += "SpriteA";
                    }
                    else if ((currentVariant % differentFormVariationsTotal) == 9)
                    {
                        if (differentFormVariation != 1)
                        {
                            skipArtAddition = true;
                        }
                        spriteName += "SpriteB";
                    }
                    else if ((currentVariant % differentFormVariationsTotal) == 10)
                    {
                        if (differentFormVariation != 1)
                        {
                            skipArtAddition = true;
                        }
                        spriteName += "FootPrint";
                    }
                }
                else
                {
                    if ((currentVariant % differentFormVariationsTotal) < 8)
                    {
                        spriteName += Variant(currentVariant % 4);
                    }
                    else if ((currentVariant % differentFormVariationsTotal) == 8)
                    {
                        spriteName += "SpriteA";
                    }
                    else if ((currentVariant % differentFormVariationsTotal) == 9)
                    {
                        spriteName += "SpriteB";
                    }
                    else if ((currentVariant % differentFormVariationsTotal) == 10)
                    {
                        if(differentFormVariation != 0)
                        {
                            skipArtAddition = true;
                        }
                        spriteName += "FootPrint";
                    }
                }
            }
            else
            {
                if (currentVariant < 8)
                {
                    spriteName += Variant(currentVariant % 4);
                }
                else if (currentVariant == 8)
                {
                    spriteName += "SpriteA";
                }
                else if (currentVariant == 9)
                {
                    spriteName += "SpriteB";
                }
                else if (currentVariant == 10)
                {
                    spriteName += "FootPrint";
                }
            }


            currentVariant++;
            if (currentVariant >= totalArtSlices)
            {
                pokedexNumber++;
                currentVariant = 0;
            }
            //else if (currentVariant >= 8 && hasDifferentGenderArt == true)
            //{
            //    currentVariant = 0;
            //}
            Debug.Log(spriteName);
            if(skipArtAddition == false)
            {
                meta.name = spriteName;
                metas.Add(meta);
            }
        }

        importer.spritesheet = metas.ToArray();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    List<Rect> SliceUpPokemonToOwnRects(List<Rect> rects, int textureWidth, int textureHeight)
    {
        List<Rect> list = new List<Rect>();

        int additions = 0;
        foreach (int i in differentGenderArt)
        {
            additions += PokemonAlternativeFormNamesForArt(i).Length -1;
        }

        for (int i = 0; i < (endingInteger + additions) - (startingInteger - 1); i++)
        {
            int XPos = (int)startingSlicingPos.x + ((int)pokemonArtMainSize.x * (i % howManyPerLine));
            int YPos = textureHeight - (int)startingSlicingPos.y - ((int)pokemonArtMainSize.y * ((i / howManyPerLine) + 1));

            list.Add(new Rect(XPos, YPos, pokemonArtMainSize.x, pokemonArtMainSize.y));
        }

        return list;
    }

    List<Rect> IndividualizeEachPokemonSlice(List<Rect> slicedUpList)
    {
        List<Rect> list = new List<Rect>();
        int currentPokemon = 0;
        bool hasDifferentGenderArt = false;

        for (int i = 0; i < slicedUpList.Count; i++)
        {
            //Debug.Log(currentPokemon + startingInteger);
            //if (hasDifferentGenderArt == false)
            //{
            //    hasDifferentGenderArt = true;
            //}
            //else
            //{
            //    hasDifferentGenderArt = false;
            //}

            //Regular

            int XPos = (int)slicedUpList[i].x + (int)pokemonFrontAPos.x;
            int YPos = (int)slicedUpList[i].y + (int)pokemonFrontAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            XPos = (int)slicedUpList[i].x + (int)pokemonFrontBPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonFrontBPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            XPos = (int)slicedUpList[i].x + (int)pokemonBackAPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonBackAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            XPos = (int)slicedUpList[i].x + (int)pokemonBackBPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonBackBPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            //Shiny

            XPos = (int)slicedUpList[i].x + (int)pokemonShinyFrontAPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonShinyFrontAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            XPos = (int)slicedUpList[i].x + (int)pokemonShinyFrontBPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonShinyFrontBPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            XPos = (int)slicedUpList[i].x + (int)pokemonShinyBackAPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonShinyBackAPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            XPos = (int)slicedUpList[i].x + (int)pokemonShinyBackBPos.x;
            YPos = (int)slicedUpList[i].y + (int)pokemonShinyBackBPos.y;

            list.Add(new Rect(XPos, YPos, pokemonArtStandardSize.x, pokemonArtStandardSize.y));

            if (hasDifferentGenderArt == false)
            {
                //Sprite

                XPos = (int)slicedUpList[i].x + (int)pokemonSpriteAPos.x;
                YPos = (int)slicedUpList[i].y + (int)pokemonSpriteAPos.y;

                list.Add(new Rect(XPos, YPos, pokemonArtSpriteSize.x, pokemonArtSpriteSize.y));

                XPos = (int)slicedUpList[i].x + (int)pokemonSpriteBPos.x;
                YPos = (int)slicedUpList[i].y + (int)pokemonSpriteBPos.y;

                list.Add(new Rect(XPos, YPos, pokemonArtSpriteSize.x, pokemonArtSpriteSize.y));

                //Footprint

                XPos = (int)slicedUpList[i].x + (int)pokemonFootprintPos.x;
                YPos = (int)slicedUpList[i].y + (int)pokemonFootprintPos.y;

                list.Add(new Rect(XPos, YPos, pokemonArtFootprintSize.x, pokemonArtFootprintSize.y));

                currentPokemon++;
            }
        }
        return list;
    }

    static string[] PokemonAlternativeFormNamesForArt(int pokedexNumber)
    {
        if (PokemonNameList.GenderExclusive(pokedexNumber))
        {
            return new string[] { "Male", "Female" };
        }
        else if (pokedexNumber == 386)
        {
            return new string[] { "Normal", "Attack", "Defense", "Speed" };
        }
        else if (pokedexNumber == 412 || pokedexNumber == 413)
        {
            return new string[] { "Plant", "Sandy", "Trash" };
        }
        else if (pokedexNumber == 421)
        {
            return new string[] { "Overcast", "Sunshine" };
        }
        else if (pokedexNumber == 422 || pokedexNumber == 423)
        {
            return new string[] { "West", "East" };
        }
        else if (pokedexNumber == 479)
        {
            return new string[] { "", "Heat", "Wash", "Frost", "Fan", "Mow" };
        }
        else if (pokedexNumber == 487)
        {
            return new string[] { "Altered", "Origin" };
        }
        else if (pokedexNumber == 492)
        {
            return new string[] { "Land", "Sky" };
        }
        else if (pokedexNumber == 493)
        {
            return PokemonNameList.ArceusForm;
        }
        return null;
    }
}