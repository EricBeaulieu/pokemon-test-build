using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirections { Up,Down,Left,Right}

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float movementSpeed = 5f;
    protected LayerMask solidObjectLayermask;
    protected LayerMask interactableLayermask;
    protected LayerMask grassLayermask;
    protected LayerMask playerLayerMask;

    protected bool _isMoving;
    bool _isRunning;

    protected Animator _anim;

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

    #endregion

    protected virtual void Awake()
    {
        _anim = GetComponentInChildren<Animator>();

        solidObjectLayermask = LayerMask.GetMask("SolidObjects");
        interactableLayermask = LayerMask.GetMask("Interactable");
        grassLayermask = LayerMask.GetMask("Grass");
        playerLayerMask = LayerMask.GetMask("Player");

        CorrectStartingPlacement();
    }

    public abstract void HandleUpdate();

    virtual protected IEnumerator MoveToPosition(Vector2 moveVector)
    {
        if( moveVector == Vector2.zero)
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

        _isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        _isMoving = false;
        //_isRunning = false;

        //_anim.SetBool("isMoving", _isMoving);
        //_anim.SetBool("isRunning", isRunning);
    }

    protected bool CheckIfWalkable(Vector3 targetPos)
    {
        Vector2 targetPositionFixed = new Vector2(Mathf.FloorToInt(targetPos.x) + TILE_CENTER_OFFSET, Mathf.FloorToInt(targetPos.y) + TILE_CENTER_OFFSET);

        if (Physics2D.OverlapCircle(targetPositionFixed, 0.25f, solidObjectLayermask | interactableLayermask | playerLayerMask) != null)
        {
            return false;
        }

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

    void CorrectStartingPlacement()
    {
        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.FloorToInt(currentPos.x) + TILE_CENTER_OFFSET;
        currentPos.y = Mathf.FloorToInt(currentPos.y) + TILE_CENTER_OFFSET;
        currentPos.z = 0;

        transform.position = currentPos;
    }

}
