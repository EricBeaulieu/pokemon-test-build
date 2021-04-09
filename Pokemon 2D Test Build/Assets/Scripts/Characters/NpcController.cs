using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : Entity, IInteractable
{
    [SerializeField] Dialog dialog;

    [SerializeField] List<Vector2> movementPattern;
    int _currentMovementPattern = 0;

    float _idleTimer = 0f;
    float _idleTimerLimit = 0f;

    [Tooltip("This is the amount of tim e the NPC will sit and when finished they will move")]
    [SerializeField] float timeUntilMoveMin;
    [Tooltip("This is to add a random timer to the Idle amount time, if this is less then the min then there will be no random range timer")]
    [SerializeField] float timeUntilMoveMax;

    bool _interactWhenPossible;


    protected override void Awake()
    {
        base.Awake();

        if(timeUntilMoveMin < 0)
        {
            timeUntilMoveMin = 0;
        }

        movementPattern = CheckNPCStartingPath(movementPattern);
        _interactWhenPossible = false;
    }

    public override void HandleUpdate()
    {
        if(IsMoving == false)
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

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    IEnumerator Walk()
    {
        Vector2 desiredPosition = movementPattern[_currentMovementPattern] - (Vector2)transform.position;

        while ((Vector2)transform.position != movementPattern[_currentMovementPattern])
        {
            if (_interactWhenPossible == true)
            {
                _interactWhenPossible = false;

                _anim.SetBool("isMoving", IsMoving);
                _anim.SetBool("isRunning", isRunning);

                yield break;
            }
            yield return MoveToPosition(desiredPosition.normalized);
        }
        _currentMovementPattern = (_currentMovementPattern + 1) % movementPattern.Count;

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
    }



    void IInteractable.OnInteract(Vector2 initiator)
    {
        if(IsMoving == false)
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

        Vector2 currentPos = (Vector2)transform.position;

        //AIDecision splitDecisions;
        //Vector2 path;
        //Vector2 directionToFace;
        //float specificTime;

        foreach (Vector2 path in currentPath)
        {

            if (path.x != 0 && path.y != 0)
            {
                Vector2 pathX = new Vector2(path.x, 0);
                currentPos += pathX;
                copyOfCurrentPath.Add(currentPos);

                Vector2 pathY = new Vector2(0, path.y);
                currentPos += pathY;
                copyOfCurrentPath.Add(currentPos);
                continue;
            }
            currentPos += path;
            copyOfCurrentPath.Add(currentPos);
        }

        for (int i = 0; i < copyOfCurrentPath.Count; i++)
        {
            if(i == 0)
            {
                isPathClear(transform.position, copyOfCurrentPath[i]);
            }
            else
            {
                isPathClear(copyOfCurrentPath[i-1], copyOfCurrentPath[i]);
            }
        }

        return copyOfCurrentPath;
    }

    void isPathClear(Vector2 startPosition ,Vector2 targetDestination)
    {
        Debug.DrawLine(startPosition, targetDestination, Color.magenta, 5f);

        RaycastHit2D hit = Physics2D.Linecast(startPosition, targetDestination, solidObjectLayermask | interactableLayermask | playerLayerMask);

        if (hit == true && hit.transform != this.transform)
        {
            Debug.Log($"Obstruction detected in this NPC Path along start {hit.transform.gameObject}", gameObject);
        }
    }

    public override void PlayerInteractingWithWhenDoneMoving()
    {
        _interactWhenPossible = true;
    }
}