using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    protected LayerMask solidObjectLayermask;
    protected LayerMask interactableLayermask;
    protected LayerMask grassLayermask;

    protected bool _isMoving;
    bool _isRunning;

    protected Animator _anim;

    const float TILE_CENTER_OFFSET = 0.5f;
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

    void Awake()
    {
        _anim = GetComponentInChildren<Animator>();

        solidObjectLayermask = LayerMask.GetMask("SolidObjects");
        interactableLayermask = LayerMask.GetMask("Interactable");
        grassLayermask = LayerMask.GetMask("Grass");
    }

    virtual protected IEnumerator MoveToPosition(Vector2 moveVector)
    {
        Vector3 targetPos = transform.position;

        targetPos.x += Mathf.RoundToInt(moveVector.x);
        targetPos.y += Mathf.RoundToInt(moveVector.y);

        _anim.SetFloat("moveX", Mathf.Clamp(moveVector.x,-1f,1f));
        _anim.SetFloat("moveY", Mathf.Clamp(moveVector.y, -1f, 1f));

        if(CheckIfWalkable(targetPos) == false)
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
    }

    bool CheckIfWalkable(Vector3 targetPos)
    {
        Vector2 targetPositionFixed = new Vector2(Mathf.FloorToInt(targetPos.x) + TILE_CENTER_OFFSET, Mathf.FloorToInt(targetPos.y) + TILE_CENTER_OFFSET);

        if (Physics2D.OverlapCircle(targetPositionFixed, 0.25f, solidObjectLayermask | interactableLayermask) != null)
        {
            return false;
        }

        return true;
    }

}
