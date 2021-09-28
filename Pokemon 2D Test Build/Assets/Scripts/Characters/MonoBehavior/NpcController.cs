using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : Entity, IInteractable
{
    [SerializeField] NPCBaseSO nPCBase;

    [Tooltip("If true it will utilize all of the can look directions, if false it will utilize the aiDecisionList")]
    [SerializeField] bool standAroundAndLook = true;
    [SerializeField] bool canLookUp = true;
    [SerializeField] bool canLookDown = true;
    [SerializeField] bool canLookLeft = true;
    [SerializeField] bool canLookRight = true;

    [SerializeField] List<AIDecision> aiDecisionList;
    bool currentlyExecutingDecision = false;
    int _currentMovementPattern = 0;
    float _idleTimer = 0f;
    float _idleTimerLimit = 0f;
    [Tooltip("This is the amount of time the NPC will sit and when finished they will move")]
    [SerializeField] float timeUntilMoveMin = 1f;
    [Tooltip("This is to add a random timer to the Idle amount time, if this is less then the min then there will be no random range timer")]
    [SerializeField] float timeUntilMoveMax = 3f;

    bool _interactWhenPossible;
    ItemGiver itemGiver;

    void Awake()
    {
        base.Initialization(nPCBase);

        if(timeUntilMoveMin < 0)
        {
            timeUntilMoveMin = 0;
        }

        if (nPCBase == null)
        {
            Debug.LogWarning("This NPC has no Data Entered", gameObject);
            return;
        }

        itemGiver = GetComponent<ItemGiver>();
        aiDecisionList = CheckNPCStartingDecisions(aiDecisionList);
        _interactWhenPossible = false;
    }

    public override void HandleUpdate()
    {
        if(IsMoving == false && currentlyExecutingDecision == false)
        {
            _idleTimer += Time.deltaTime;

            if(_idleTimer >= _idleTimerLimit)
            {
                _idleTimer = 0;

                if (standAroundAndLook == true)
                {
                    FaceTowardsDirection(LookTowards());
                    _idleTimerLimit = SetNewIdleTimer();
                }
                else if (aiDecisionList.Count >0)
                {
                    if(aiDecisionList[_currentMovementPattern].movement != Vector2.zero)
                    {
                        StartCoroutine(Walk());
                    }
                    else if(aiDecisionList[_currentMovementPattern].directionToFace != Vector2.zero)
                    {
                        base.FaceTowardsDirection(aiDecisionList[_currentMovementPattern].directionToFace + (Vector2)transform.position);
                        _currentMovementPattern = (_currentMovementPattern + 1) % aiDecisionList.Count;
                    }

                    if(aiDecisionList[_currentMovementPattern].specificTimeUniltNextExecution > 0)
                    {
                        _idleTimerLimit = aiDecisionList[_currentMovementPattern].specificTimeUniltNextExecution;
                        _currentMovementPattern = (_currentMovementPattern + 1) % aiDecisionList.Count;
                    }
                    else
                    {
                        _idleTimerLimit = SetNewIdleTimer();
                    }
                }
                else
                {
                    _idleTimerLimit = SetNewIdleTimer();
                }
            }
        }

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    IEnumerator Walk()
    {
        Vector2 desiredPosition = aiDecisionList[_currentMovementPattern].movement - (Vector2)transform.position;
        currentlyExecutingDecision = true;

        while ((Vector2)transform.position != aiDecisionList[_currentMovementPattern].movement)
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
        currentlyExecutingDecision = false;
        _currentMovementPattern = (_currentMovementPattern + 1) % aiDecisionList.Count;

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    public IEnumerator OnInteract(Vector2 initiator)
    {
        if(IsMoving == false)
        {
            currentlyExecutingDecision = false;
            FaceTowardsDirection(initiator);

            if(itemGiver != null && itemGiver.ItemCanBeGiven() == true)
            {
                yield return itemGiver.GiveItem(GameManager.instance.GetPlayerController);
            }
            else
            {
                yield return GameManager.instance.GetDialogSystem.ShowDialogBox(nPCBase.GetDialog);
            }
            _idleTimer = 0;
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

    List<AIDecision> CheckNPCStartingDecisions(List<AIDecision> currentDecisions)
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

            if(path != Vector2.zero)
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

            if(directionToFace != Vector2.zero)
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

            if(specificTime > 0)
            {
                copyOfCurrentPath.Add(new AIDecision(specificTime));
            }
        }

        Vector2 previousMovement = transform.position;
        for (int i = 0; i < copyOfCurrentPath.Count; i++)
        {
            if(copyOfCurrentPath[i].movement != Vector2.zero)
            {
                isPathClear(previousMovement, copyOfCurrentPath[i].movement);
                previousMovement = copyOfCurrentPath[i].movement;
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

    public NPCBaseSO GetNPCBase
    {
        get { return nPCBase; }
    }

    FacingDirections LookTowards()
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
}