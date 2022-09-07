using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    string _spriteName;
    public FacingDirections currentDirection { get; private set; }
    SpriteRenderer _spriteRenderer;
    Animator _anim;

    [SerializeField] List<Sprite> testSpriteSheet;

    private void Start()
    {
        Sprite newSpriteSheet = GetComponentInParent<Entity>().CharacterArt.GetOverworldSpriteSheet;
        _spriteName = newSpriteSheet.name.Split('_')[0];
        if (newSpriteSheet == null)
        {
            Debug.LogError("This GameObject requires a sprite sheet to be overriden", gameObject);
            Destroy(this);
            return;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();

        GlobalArt.AddSpriteSheetToList(newSpriteSheet);
    }

    public void SetCurrentDirection(FacingDirections facing)
    {
        currentDirection = facing;
    }

    /// <summary>
    /// This acts as an animator for the player, this will find the specified name in a sprite atlas and return it
    /// </summary>
    /// <param name="name">Specified name of the animation youre looking for (Idle,Walk,Run,Bike,Surf,Fish ** HM,Item,BikeJump)</param>
    /// <param name="variation">-1 is defaulted as it doesnt exits</param>
    /// <param name="specialized">HM,Item,BikeJump animations are specialized</param>
    void PlaySprite(string name = "Idle", int variation = -1, bool specialized = false)
    {
        string currentSpriteName = _spriteName;
        
        if (specialized == false)
        {
            currentSpriteName += $"_{currentDirection}";
        }

        currentSpriteName += $"_{name}";

        if (variation >= 0)
        {
            currentSpriteName += $"_{variation}";
        }

        _spriteRenderer.sprite = GlobalArt.GetSprite(currentSpriteName);
    }

    #region AnimationEvents

    #endregion
    public void Idle()
    {
        Debug.Log($"played {currentDirection}");
        PlaySprite();
    }

    public void Walk(int variation)
    {
        PlaySprite("Walk",variation);
    }

    public void Run(int variation)
    {
        Debug.Log($"Run {variation}");
        PlaySprite("Run", variation);
    }
}
