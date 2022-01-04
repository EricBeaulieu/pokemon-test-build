using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAI : Entity
{
    [Tooltip("If true it will utilize all of the can look directions, if false it will utilize the aiDecisionList")]
    [SerializeField] protected bool standAroundAndLook = true;
    [SerializeField] protected bool canLookUp = true;
    [SerializeField] protected bool canLookDown = true;
    [SerializeField] protected bool canLookLeft = true;
    [SerializeField] protected bool canLookRight = true;

    [SerializeField] protected bool lookDirectionAfterInteraction = false;
    [SerializeField] protected FacingDirections directionAfterInteraction;

    [SerializeField] protected List<AIDecision> aiDecisionList;
    [SerializeField] protected Vector2Int pathLocation;
    protected bool currentlyExecutingDecision = false;
    protected int currentMovementPattern = 0;
    protected float idleTimer = 0f;
    protected float idleTimerLimit = 0f;
    [Tooltip("This is the amount of time the AI will sit and when finished they will move")]
    [SerializeField] protected float timeUntilMoveMin = 1f;
    [Tooltip("This is to add a random timer to the Idle amount time, if this is less then the min then there will be no random range timer")]
    [SerializeField] protected float timeUntilMoveMax = 3f;

    protected bool interactWhenPossible;

    protected IEnumerator Walk()
    {
        Vector2 desiredPosition = aiDecisionList[currentMovementPattern].movement - (Vector2)transform.position;

        //while(CheckIfWalkable(desiredPosition.normalized) == false)
        //{
        //    yield return null;
        //}
        currentlyExecutingDecision = true;

        while ((Vector2)transform.position != aiDecisionList[currentMovementPattern].movement)
        {
            if (interactWhenPossible == true)
            {
                interactWhenPossible = false;

                _anim.SetBool("isMoving", IsMoving);
                _anim.SetBool("isRunning", isRunning);

                yield break;
            }
            yield return MoveToPosition(desiredPosition.normalized);
        }
        currentlyExecutingDecision = false;
        currentMovementPattern = (currentMovementPattern + 1) % aiDecisionList.Count;

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    protected IEnumerator Walk(List<Vector2> path)
    {
        currentlyExecutingDecision = true;

        for (int i = 0; i < path.Count; i++)
        {
            yield return MoveToPosition(path[i]);
        }
        currentlyExecutingDecision = false;

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    protected FacingDirections LookTowards()
    {
        if (canLookUp == false && canLookDown == false && canLookLeft == false && canLookRight == false)
        {
            Debug.LogError("Trainer has been set to not look towards any direction", gameObject);
            return FacingDirections.Down;
        }

        FacingDirections facing = FacingDirections.Down;
        bool directionFound = false;

        while (directionFound == false)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    if (canLookUp == true)
                    {
                        facing = FacingDirections.Up;
                        directionFound = true;
                    }
                    break;
                case 1:
                    if (canLookDown == true)
                    {
                        facing = FacingDirections.Down;
                        directionFound = true;
                    }
                    break;
                case 2:
                    if (canLookLeft == true)
                    {
                        facing = FacingDirections.Left;
                        directionFound = true;
                    }
                    break;
                default:
                    if (canLookRight == true)
                    {
                        facing = FacingDirections.Right;
                        directionFound = true;
                    }
                    break;
            }
        }

        return facing;
    }

    protected float SetNewIdleTimer()
    {
        if (timeUntilMoveMin >= timeUntilMoveMax)
        {
            return timeUntilMoveMin;
        }
        else
        {
            return Random.Range(timeUntilMoveMin, timeUntilMoveMax);
        }
    }

    protected List<AIDecision> CheckAIDecisions(List<AIDecision> currentDecisions)
    {
        List<AIDecision> copyOfCurrentPath = new List<AIDecision>();

        Vector2 currentPos = (Vector2)transform.position;

        Vector2 path;
        Vector2 directionToFace;
        float specificTime;

        foreach (AIDecision decision in currentDecisions)
        {
            path = decision.movement;
            directionToFace = decision.directionToFace;
            specificTime = decision.specificTimeUniltNextExecution;

            if (path != Vector2.zero)
            {
                if (path.x != 0 && path.y != 0)
                {
                    Vector2 pathX = new Vector2(path.x, 0);
                    currentPos += pathX;
                    copyOfCurrentPath.Add(new AIDecision(currentPos));

                    Vector2 pathY = new Vector2(0, path.y);
                    currentPos += pathY;
                    copyOfCurrentPath.Add(new AIDecision(currentPos));
                }
                else
                {
                    currentPos += path;
                    copyOfCurrentPath.Add(new AIDecision(currentPos));
                }
            }

            if (directionToFace != Vector2.zero)
            {
                if (directionToFace.x > 0)
                {
                    copyOfCurrentPath.Add(new AIDecision(FacingDirections.Right));
                }
                else if (directionToFace.x < 0)
                {
                    copyOfCurrentPath.Add(new AIDecision(FacingDirections.Left));
                }

                if (directionToFace.y > 0)
                {
                    copyOfCurrentPath.Add(new AIDecision(FacingDirections.Up));
                }
                else if (directionToFace.y < 0)
                {
                    copyOfCurrentPath.Add(new AIDecision(FacingDirections.Down));
                }
            }

            if (specificTime > 0)
            {
                copyOfCurrentPath.Add(new AIDecision(specificTime));
            }
        }

        Vector2 previousMovement = transform.position;
        for (int i = 0; i < copyOfCurrentPath.Count; i++)
        {
            if (copyOfCurrentPath[i].movement != Vector2.zero)
            {
                isPathClear(previousMovement, copyOfCurrentPath[i].movement);
                previousMovement = copyOfCurrentPath[i].movement;
            }
        }

        return copyOfCurrentPath;
    }

    void isPathClear(Vector2 startPosition, Vector2 targetDestination)
    {
        Debug.DrawLine(startPosition, targetDestination, Color.red, 5f);

        RaycastHit2D hit = Physics2D.Linecast(startPosition, targetDestination, solidObjectLayermask | interactableLayermask | playerLayerMask);

        if (hit == true && hit.transform != this.transform)
        {
            Debug.Log($"Obstruction detected in this Trainer Path along start {hit.transform.gameObject}", gameObject);
        }
    }

    public override void PlayerInteractingWithWhenDoneMoving()
    {
        interactWhenPossible = true;
    }

    public void GeneratePathToPosition(Vector2 target = new Vector2())
    {
        if (target == Vector2.zero)
        {
            target = new Vector2(pathLocation.x + TILE_CENTER_OFFSET, pathLocation.y + TILE_CENTER_OFFSET);
        }
        List<Vector2> path = ArtificialGrid.Pathfinding.FindPath((Vector2)transform.position, target);
        if (path != null)
        {
            StartCoroutine(Walk(path));
        }
        else
        {
            Debug.Log("Path is null");
        }
    }
}
