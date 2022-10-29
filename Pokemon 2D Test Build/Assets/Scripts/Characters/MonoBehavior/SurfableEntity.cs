using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfableEntity : MonoBehaviour
{
    Dictionary<string, Sprite> _spriteSheet;
    SpriteRenderer _spriteRenderer;
    [SerializeField] List<Sprite> surfablesSpriteSheet;
    FacingDirections currentDirection;
    Animator _anim;
    /// <summary>
    ///This bool is if the player is animating on or off of the surfable pokemon
    /// </summary>
    bool playerAnimating = false;
    SpriteSheetSwap entityGraphics;

    const float XPOS_OFFSET = 0.2f;
    const float YPOS_OFFSET = 0.3f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteSheet = LoadNewSpriteSheet(surfablesSpriteSheet);
        _anim = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        _spriteRenderer.sprite = _spriteSheet[_spriteRenderer.sprite.name];
    }

    public void SetCharactersSpriteSwap(SpriteSheetSwap graphics,FacingDirections dir)
    {
        entityGraphics = graphics;
        SetAnimator(dir);
    }
    /// <summary>
    /// removes the character reference, stopping it from bobing while riding it
    /// </summary>
    public void SetCharactersSpriteSwap()
    {
        entityGraphics = null;
    }

    public void SnaptoDirection(FacingDirections directions)
    {
        if(directions == currentDirection)
        {
            return;
        }

        switch (directions)
        {
            case FacingDirections.Up:
                transform.localPosition = new Vector2(0, YPOS_OFFSET);
                _spriteRenderer.sortingOrder = -1;
                break;
            case FacingDirections.Down:
                transform.localPosition = new Vector2(0, -YPOS_OFFSET);
                _spriteRenderer.sortingOrder = 1;
                break;
            case FacingDirections.Left:
                transform.localPosition = new Vector2(-XPOS_OFFSET, 0);
                _spriteRenderer.sortingOrder = -1;
                break;
            default://FacingDirections.Right
                transform.localPosition = new Vector2(XPOS_OFFSET, 0);
                _spriteRenderer.sortingOrder = -1;
                break;
        }

        SetAnimator(directions);
    }

    /// <summary>
    /// called on the animator
    /// </summary>
    /// <param name="y"></param>
    void AdjustRiderYGraphicsLevel(float y)
    {
        if(playerAnimating == true)
        {
            return;
        }

        if(entityGraphics != null)
        entityGraphics.SnapGraphicsToOffsetYPos(y + Entity.ENTITY_Y_OFFSET);
    }

    Dictionary<string, Sprite> LoadNewSpriteSheet(List<Sprite> specifiedSprites)
    {
        Dictionary<string, Sprite> newSheet = new Dictionary<string, Sprite>();

        newSheet.Add("Up0", specifiedSprites[0]);
        newSheet.Add("Up1", specifiedSprites[1]);
        newSheet.Add("Down0", specifiedSprites[2]);
        newSheet.Add("Down1", specifiedSprites[3]);
        newSheet.Add("Left0", specifiedSprites[4]);
        newSheet.Add("Left1", specifiedSprites[5]);
        newSheet.Add("Right0", specifiedSprites[6]);
        newSheet.Add("Right1", specifiedSprites[7]);

        return newSheet;
    }

    void SetAnimator(FacingDirections dir)
    {
        Vector2 moveVector = GlobalTools.CurrentDirectionFacing(dir);
        _anim.SetFloat("moveX", Mathf.Clamp(moveVector.x, -1f, 1f));
        _anim.SetFloat("moveY", Mathf.Clamp(moveVector.y, -1f, 1f));
        currentDirection = dir;
    }

    public void SetPositionPlusOffset(Vector2 newPosition, FacingDirections directions)
    {
        switch (directions)
        {
            case FacingDirections.Up:
                transform.position = newPosition + new Vector2(0, YPOS_OFFSET);
                _spriteRenderer.sortingOrder = -1;
                break;
            case FacingDirections.Down:
                transform.position = newPosition + new Vector2(0, -YPOS_OFFSET);
                _spriteRenderer.sortingOrder = 1;
                break;
            case FacingDirections.Left:
                transform.position = newPosition + new Vector2(-XPOS_OFFSET, 0);
                _spriteRenderer.sortingOrder = -1;
                break;
            default://FacingDirections.Right
                transform.position = newPosition + new Vector2(XPOS_OFFSET, 0);
                _spriteRenderer.sortingOrder = -1;
                break;
        }

        SetAnimator(directions);
    }

}
