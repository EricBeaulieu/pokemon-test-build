using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcState { Idle, Walking}

public class NpcController : Entity, IInteractable
{
    [SerializeField] Dialog dialog;

    [SerializeField] List<Vector2> movementPattern;
    int _currentMovementPattern = 0;

    NpcState _state;
    float _idleTimer = 0f;
    float _idleTimerLimit = 0f;

    [Tooltip("This is the amount of tim e the NPC will sit and when finished they will move")]
    [SerializeField] float timeUntilMoveMin;
    [Tooltip("This is to add a random timer to the Idle amount time, if this is less then the min then there will be no random range timer")]
    [SerializeField] float timeUntilMoveMax;


    protected override void Awake()
    {
        base.Awake();

        _state = NpcState.Idle;

        if(timeUntilMoveMin < 0)
        {
            timeUntilMoveMin = 0;
        }

        movementPattern = CheckNPCStartingPath(movementPattern);
    }

    public override void HandleUpdate()
    {
        if(_state == NpcState.Idle)
        {
            _idleTimer += Time.deltaTime;

            if(_idleTimer >= _idleTimerLimit)
            {
                _idleTimer = 0;
                _idleTimerLimit = SetNewIdleTimer();

                if(movementPattern.Count >0)
                {
                    StartCoroutine(Walk());
                }
            }
        }

        _anim.SetBool("isMoving", _isMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    IEnumerator Walk()
    {
        _state = NpcState.Walking;

        Vector2 desiredPosition = movementPattern[_currentMovementPattern] + (Vector2)transform.position;

        while ((Vector2)transform.position != desiredPosition)
        {
            yield return MoveToPosition(movementPattern[_currentMovementPattern].normalized);
        }
        _currentMovementPattern = (_currentMovementPattern + 1) % movementPattern.Count;

        _anim.SetBool("isMoving", _isMoving);
        _anim.SetBool("isRunning", isRunning);

        _state = NpcState.Idle;
    }

    void IInteractable.OnInteract(Vector2 initiator)
    {
        if(_state == NpcState.Idle)
        {
            FaceTowardsDirection(initiator);
            StartCoroutine(DialogManager.instance.ShowDialogBox(dialog, () =>
            {
                _idleTimer = 0;
            }));
        }
    }

    float SetNewIdleTimer()
    {
        if(timeUntilMoveMin >= timeUntilMoveMax)
        {
            return timeUntilMoveMin;
        }
        else
        {
            return Random.Range(timeUntilMoveMin, timeUntilMoveMax);
        }
    }

    List<Vector2> CheckNPCStartingPath(List<Vector2> currentPath)
    {
        List<Vector2> copyOfCurrentPath = new List<Vector2>();

        foreach (Vector2 path in currentPath)
        {
            if (path.x != 0 && path.y != 0)
            {
                Vector2 pathX = new Vector2(path.x, 0);
                Vector2 pathY = new Vector2(0,path.y);

                copyOfCurrentPath.Add(pathX);
                copyOfCurrentPath.Add(pathY);
                continue;
            }

            copyOfCurrentPath.Add(path);
        }

        Vector2 currentPos = (Vector2)transform.position;
        for (int i = 0; i < copyOfCurrentPath.Count; i++)
        {
            isPathClear(currentPos, copyOfCurrentPath[i]);
            currentPos += copyOfCurrentPath[i];
        }

        return copyOfCurrentPath;
    }

    void isPathClear(Vector2 startPosition ,Vector2 targetDestination)
    {
        var targetPos = targetDestination + startPosition;
        var dir = targetDestination.normalized;

        Debug.DrawLine(startPosition, targetPos, Color.magenta, 5f);

        RaycastHit2D hit = Physics2D.Linecast((Vector2)startPosition + dir, targetPos, solidObjectLayermask | interactableLayermask | playerLayerMask);

        if (hit == true && hit.transform != this.transform)
        {
            Debug.Log($"Obstruction detected in this NPC Path along start {hit.transform.gameObject}", gameObject);
        }
    }
}