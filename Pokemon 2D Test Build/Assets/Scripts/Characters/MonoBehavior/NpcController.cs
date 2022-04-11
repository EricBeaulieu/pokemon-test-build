using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : EntityAI, IInteractable
{
    [Header("Npc Controller")]
    [SerializeField] Dialog dialog;
    ItemGiver itemGiver;

    void Awake()
    {
        base.Initialization();

        if(timeUntilMoveMin < 0)
        {
            timeUntilMoveMin = 0;
        }

        if (dialog.Lines.Count <= 0)
        {
            Debug.LogError("This NPC is missing its dialog", this);
        }

        itemGiver = GetComponent<ItemGiver>();
        aiDecisionList = CheckAIDecisions(aiDecisionList);
        interactWhenPossible = false;
    }

    public override void HandleUpdate()
    {
        if(IsMoving == false && currentlyExecutingDecision == false)
        {
            idleTimer += Time.deltaTime;

            if(idleTimer >= idleTimerLimit)
            {
                idleTimer = 0;

                if (standAroundAndLook == true)
                {
                    FaceTowardsDirection(LookTowards());
                    idleTimerLimit = SetNewIdleTimer();
                }
                else if (aiDecisionList.Count >0)
                {
                    if(aiDecisionList[currentMovementPattern].movement != Vector2.zero)
                    {
                        StartCoroutine(Walk());
                    }
                    else if(aiDecisionList[currentMovementPattern].directionToFace != Vector2.zero)
                    {
                        base.FaceTowardsDirection(aiDecisionList[currentMovementPattern].directionToFace + (Vector2)transform.position);
                        currentMovementPattern = (currentMovementPattern + 1) % aiDecisionList.Count;
                    }

                    if(aiDecisionList[currentMovementPattern].specificTimeUniltNextExecution > 0)
                    {
                        idleTimerLimit = aiDecisionList[currentMovementPattern].specificTimeUniltNextExecution;
                        currentMovementPattern = (currentMovementPattern + 1) % aiDecisionList.Count;
                    }
                    else
                    {
                        idleTimerLimit = SetNewIdleTimer();
                    }
                }
                else
                {
                    idleTimerLimit = SetNewIdleTimer();
                }
            }
        }

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    public IEnumerator OnInteract(Vector2 initiator)
    {
        if(IsMoving == true)
        {
            //currentlyExecutingDecision = false;
            //currentMovementPattern--;
            yield break;
        }

        FaceTowardsDirection(initiator);

        if (itemGiver != null && itemGiver.ItemCanBeGiven() == true)
        {
            yield return itemGiver.GiveItem(GameManager.instance.GetPlayerController);
        }
        else
        {
            yield return GameManager.instance.GetDialogSystem.ShowDialogBox(dialog);
        }
        idleTimer = 0;

        if (lookDirectionAfterInteraction == true)
        {
            FaceTowardsDirection(directionAfterInteraction);
        }
    }
}