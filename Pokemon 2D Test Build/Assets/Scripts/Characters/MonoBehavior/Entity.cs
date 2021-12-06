using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirections { Up,Down,Left,Right}

public abstract class Entity : MonoBehaviour
{
    [SerializeField] Transform graphics;
    [SerializeField] CharacterArtSO characterArt;
    [SerializeField] protected float movementSpeed = STANDARD_WALKING_SPEED;
    internal LayerMask solidObjectLayermask;
    internal LayerMask interactableLayermask;
    internal LayerMask grassLayermask;
    internal LayerMask playerLayerMask;
    internal LayerMask portalLayerMask;
    internal LayerMask southLedgeLayerMask;
    internal LayerMask eastLedgeLayerMask;
    internal LayerMask westLedgeLayerMask;
    internal LayerMask waterLayerMask;

    bool _isMoving;
    bool _isRunning;
    bool _isJumping;

    protected Animator _anim;
    /// <summary>
    /// this is here to prevent some entities walking through eachother stating that someone is currently moving to this position
    /// </summary>
    [SerializeField] protected Transform positionMovingTo;

    public const float TILE_CENTER_OFFSET = 0.5f;
    protected const float STANDARD_WALKING_SPEED = 5f;
    internal const float STANDARD_JUMPING_SPEED = 3.5f;
    protected const float STANDARD_RUNNING_SPEED = 12.5f;
    const float ENTITY_Y_OFFSET = 0.3f;
    const float ENTITY_JUMP_HEIGHT = 0.8f;

    internal Vector3 standardGraphicsSetting = new Vector3(0, ENTITY_Y_OFFSET, 0);

    #region Getters/Setters

    protected bool isRunning
    {
        get { return _isRunning; }

        set
        {
            _isRunning = value;
            if (value == true)
            {
                movementSpeed = STANDARD_RUNNING_SPEED;
            }
            else
            {
                movementSpeed = STANDARD_WALKING_SPEED;
            }
        }
    }

    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            _isMoving = value;
        }
    }

    public CharacterArtSO CharacterArt { get { return characterArt; } set { characterArt = value; } }

    #endregion

    protected virtual void Initialization()
    {
        _anim = GetComponentInChildren<Animator>();

        solidObjectLayermask = LayerMask.GetMask("SolidObjects");
        interactableLayermask = LayerMask.GetMask("Interactable");
        grassLayermask = LayerMask.GetMask("Grass");
        playerLayerMask = LayerMask.GetMask("Player");
        portalLayerMask = LayerMask.GetMask("Portal");
        southLedgeLayerMask = LayerMask.GetMask("SouthLedge");
        eastLedgeLayerMask = LayerMask.GetMask("EastLedge");
        westLedgeLayerMask = LayerMask.GetMask("WestLedge");
        waterLayerMask = LayerMask.GetMask("Water");

        SnapToGrid();

        if(positionMovingTo == null)
        {
            Debug.Log("positionMovingTo is missing its reference", gameObject);
        }
    }

    public abstract void HandleUpdate();

    virtual protected IEnumerator MoveToPosition(Vector2 moveVector)
    {
        if(moveVector == Vector2.zero)
        {
            yield break;
        }

        _anim.SetFloat("moveX", Mathf.Clamp(moveVector.x, -1f, 1f));
        _anim.SetFloat("moveY", Mathf.Clamp(moveVector.y, -1f, 1f));

        if (CheckIfWalkable(moveVector) == false)
        {
            yield break;
        }

        IsMoving = true;
        Vector3 targetPos = positionMovingTo.position;

        if(_isJumping == true)
        {
            Vector3 graphicsRef = standardGraphicsSetting;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                float height = Mathf.Abs((Vector3.Distance(targetPos, transform.position) - 1));
                height = (1 - height) * ENTITY_JUMP_HEIGHT;
                graphicsRef.y = height + ENTITY_Y_OFFSET;
                graphics.localPosition = graphicsRef;

                transform.position = Vector3.MoveTowards(transform.position, targetPos, STANDARD_JUMPING_SPEED * Time.deltaTime);
                positionMovingTo.position = targetPos;
                yield return null;
            }

            graphics.localPosition = standardGraphicsSetting;
            _isJumping = false;
        }
        else
        {
            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
                positionMovingTo.position = targetPos;
                yield return null;
            }
        }

        transform.position = targetPos;
        IsMoving = false;
    }

    protected bool CheckIfWalkable(Vector2 moveVector)
    {
        Vector2 targetPositionFixed = new Vector2(Mathf.FloorToInt(moveVector.x + transform.position.x) + TILE_CENTER_OFFSET, Mathf.FloorToInt(moveVector.y + transform.position.y) + TILE_CENTER_OFFSET);

        Collider2D collider = Physics2D.OverlapCircle(targetPositionFixed, 0.25f, solidObjectLayermask | interactableLayermask | 
            playerLayerMask | southLedgeLayerMask | eastLedgeLayerMask | westLedgeLayerMask| waterLayerMask);

        if(collider != null)
        {
            if ((southLedgeLayerMask & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer)
            {
                if (moveVector != Vector2.down)
                {
                    return false;
                }
                targetPositionFixed += moveVector;
                collider = Physics2D.OverlapCircle(targetPositionFixed, 0.25f, solidObjectLayermask | interactableLayermask | playerLayerMask | waterLayerMask);
            }
            else if ((eastLedgeLayerMask & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer)
            {
                if(moveVector != Vector2.right)
                {
                    return false;
                }
                targetPositionFixed += moveVector;
                collider = Physics2D.OverlapCircle(targetPositionFixed, 0.25f, solidObjectLayermask | interactableLayermask | playerLayerMask | waterLayerMask);
            }
            else if((westLedgeLayerMask & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer)
            {
                if (moveVector != Vector2.left)
                {
                    return false;
                }
                targetPositionFixed += moveVector;
                collider = Physics2D.OverlapCircle(targetPositionFixed, 0.25f, solidObjectLayermask | interactableLayermask | playerLayerMask | waterLayerMask);
            }
            
            if (collider != null)
            {
                return false;
            }
            _isJumping = true;
        }

        positionMovingTo.position = targetPositionFixed;
        return true;
    }

    protected virtual void FaceTowardsDirection(Vector2 targetPos)
    {
        if (targetPos == Vector2.zero)
        {
            return;
        }

        Vector2 dirToFace = targetPos - (Vector2)transform.position;
        dirToFace = dirToFace.normalized;

        if(dirToFace.x == 0 || dirToFace.y == 0)
        {
            _anim.SetFloat("moveX", dirToFace.x);
            _anim.SetFloat("moveY", dirToFace.y);
        }
        else
        {
            Debug.Log($"Error, cannot look at a diagnal target at {targetPos}", gameObject);
        }
    }

    protected virtual void FaceTowardsDirection(FacingDirections dir)
    {
        Vector2 targetDir = new Vector2().GetDirection(dir);

        if(targetDir == Vector2.zero)
        {
            return;
        }

        _anim.SetFloat("moveX", targetDir.x);
        _anim.SetFloat("moveY", targetDir.y);
    }

    public void SnapToGrid()
    {
        transform.position = GlobalTools.SnapToGrid(transform.position);
        positionMovingTo.localPosition = Vector3.zero;
    }

    public Vector2 CurrentWalkingToPos()
    {
        Vector2 newPos = positionMovingTo.position;

        newPos.x = Mathf.FloorToInt(newPos.x) + TILE_CENTER_OFFSET;
        newPos.y = Mathf.FloorToInt(newPos.y) + TILE_CENTER_OFFSET;
        return newPos;
    }

    public virtual void PlayerInteractingWithWhenDoneMoving() { }
    
}
