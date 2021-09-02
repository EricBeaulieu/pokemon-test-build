using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirections { Up,Down,Left,Right}

public abstract class Entity : MonoBehaviour
{
    CharacterArtSO characterArt;
    [SerializeField] protected float movementSpeed = STANDARD_WALKING_SPEED;
    protected LayerMask solidObjectLayermask;
    protected LayerMask interactableLayermask;
    protected LayerMask grassLayermask;
    protected LayerMask playerLayerMask;
    protected LayerMask portalLayerMask;

    bool _isMoving;
    bool _isRunning;

    protected Animator _anim;
    /// <summary>
    /// this is here to prevent some entities walking through eachother stating that someone is currently moving to this position
    /// </summary>
    [SerializeField] protected GameObject positionMovingTo;

    protected const float TILE_CENTER_OFFSET = 0.5f;
    protected const float STANDARD_WALKING_SPEED = 5f;
    protected const float STANDARD_RUNNING_SPEED = 12.5f;
    const float ENTITY_Y_OFFSET = -0.3f;

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

    public CharacterArtSO CharacterArt
    {
        get { return characterArt; }
        private set { characterArt = value; }
    }

    #endregion

    protected virtual void Initialization(EntityBaseSO entityBaseSO)
    {
        _anim = GetComponentInChildren<Animator>();

        solidObjectLayermask = LayerMask.GetMask("SolidObjects");
        interactableLayermask = LayerMask.GetMask("Interactable");
        grassLayermask = LayerMask.GetMask("Grass");
        playerLayerMask = LayerMask.GetMask("Player");
        portalLayerMask = LayerMask.GetMask("Portal");

        SnapToGrid();

        if(positionMovingTo == null)
        {
            Debug.Log("positionMovingTo is missing its reference", gameObject);
        }

        entityBaseSO.Initialization();
        CharacterArt = entityBaseSO.GetCharacterArt;
    }

    public abstract void HandleUpdate();

    virtual protected IEnumerator MoveToPosition(Vector2 moveVector)
    {
        if(moveVector == Vector2.zero)
        {
            yield break;
        }

        Vector3 targetPos = transform.position;

        targetPos.x += Mathf.RoundToInt(moveVector.x);
        targetPos.y += Mathf.RoundToInt(moveVector.y);

        _anim.SetFloat("moveX", Mathf.Clamp(moveVector.x, -1f, 1f));
        _anim.SetFloat("moveY", Mathf.Clamp(moveVector.y, -1f, 1f));

        if (CheckIfWalkable(targetPos) == false)
        {
            yield break;
        }

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            positionMovingTo.transform.position = targetPos;
            yield return null;
        }

        transform.position = targetPos;
        IsMoving = false;
    }

    protected bool CheckIfWalkable(Vector3 targetPos)
    {
        Vector2 targetPositionFixed = new Vector2(Mathf.FloorToInt(targetPos.x) + TILE_CENTER_OFFSET, Mathf.FloorToInt(targetPos.y) + TILE_CENTER_OFFSET);

        if (Physics2D.OverlapCircle(targetPositionFixed, 0.25f, solidObjectLayermask | interactableLayermask | playerLayerMask) != null)
        {
            return false;
        }

        positionMovingTo.transform.position = targetPositionFixed;
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
        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.FloorToInt(currentPos.x) + TILE_CENTER_OFFSET;
        currentPos.y = Mathf.FloorToInt(currentPos.y) + TILE_CENTER_OFFSET;
        currentPos.z = 0;

        transform.position = currentPos;
        positionMovingTo.transform.localPosition = Vector3.zero;
    }

    public Vector2 CurrentWalkingToPos()
    {
        Vector2 newPos = positionMovingTo.transform.position;

        newPos.x = Mathf.FloorToInt(newPos.x) + TILE_CENTER_OFFSET;
        newPos.y = Mathf.FloorToInt(newPos.y) + TILE_CENTER_OFFSET;
        return newPos;
    }

    public virtual void PlayerInteractingWithWhenDoneMoving() { }
    
}
