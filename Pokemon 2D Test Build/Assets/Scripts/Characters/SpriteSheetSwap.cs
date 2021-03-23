using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class SpriteSheetSwap : MonoBehaviour
{
    [SerializeField] Sprite newSpriteSheet;

    Dictionary<string, Sprite> _spriteSheet;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        if(newSpriteSheet == null)
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
}
