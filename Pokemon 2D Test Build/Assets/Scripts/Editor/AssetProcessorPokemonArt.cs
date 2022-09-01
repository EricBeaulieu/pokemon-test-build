using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System.IO;

public class AssetProcessorPokemonArt : AssetPostprocessor {

    void OnPostProcessSprites(Texture2D texture, Sprite[] sprites)
    {
        Debug.Log("Sprites: " + sprites.Length);
    }

    void OnPostProcessTexture(Texture2D texture)
    {
        Debug.Log("Texture2D: (" + texture.width + "x" + texture.height + ")");
    }
}

public class SpritePostprocessor : AssetPostprocessor
{

}
