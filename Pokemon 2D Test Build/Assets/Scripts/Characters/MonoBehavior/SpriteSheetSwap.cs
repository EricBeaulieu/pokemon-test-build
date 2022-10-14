using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class SpriteSheetSwap : MonoBehaviour
{
    Dictionary<string, Sprite> _spriteSheet;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        Sprite newSpriteSheet = GetComponentInParent<Entity>().CharacterArt.GetOverworldSpriteSheet;

        if (newSpriteSheet == null)
        {
            Debug.LogError("This GameObject requires a sprite sheet to be overriden", gameObject);
            Destroy(this);
            return;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteSheet = LoadNewSpriteSheet(newSpriteSheet);
    }

    private void LateUpdate()
    {
        _spriteRenderer.sprite = _spriteSheet[_spriteRenderer.sprite.name];
    }

    Dictionary<string, Sprite> LoadNewSpriteSheet(Sprite spriteSheet)
    {
        string loadedSpriteSheetPath = AssetDatabase.GetAssetPath(spriteSheet);

        var obj = AssetDatabase.LoadAllAssetsAtPath(loadedSpriteSheetPath);
        var sprites = obj.Where(x => x is Sprite).Cast<Sprite>();

        return sprites.ToDictionary(x => x.name, x => x);
    }

    //this is adjusted in the animators
    void SnapGraphicsToOffsetXPos(float x)
    {
        transform.localPosition = new Vector2(x, transform.localPosition.y);
    }

    void SnapGraphicsToOffsetYPos(float y)
    {
        transform.localPosition = new Vector2(transform.localPosition.x, y);
    }

    void testing(string s)
    {
        Debug.Log(s);
    }
}
