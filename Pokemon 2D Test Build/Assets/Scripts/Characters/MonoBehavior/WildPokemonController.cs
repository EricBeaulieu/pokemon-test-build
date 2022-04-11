using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildPokemonController : EntityAI,IInteractable
{
    [SerializeField] WildPokemon _pokemon;
    public WildPokemon pokemon { get { return _pokemon; } }
    [SerializeField] bool alwaysInOverworld = false;
    [SerializeField] bool legendary = false;
    [Tooltip("Will use the same feature as the trainer, it will have a line of sight and say its name and fight the player")]
    bool isAggressive = false;

    [Tooltip("Interactable but the player will never fight")]
    [SerializeField] bool interactionWillNeverFight;
    [SerializeField] Dialog dialog;

    // Start is called before the first frame update
    void Awake()
    {
        base.Initialization();

        if (timeUntilMoveMin < 0)
        {
            timeUntilMoveMin = 0;
        }

        aiDecisionList = CheckAIDecisions(aiDecisionList);
        interactWhenPossible = false;
    }

    public override void HandleUpdate()
    {

    }

    public IEnumerator OnInteract(Vector2 initiator)
    {
        //FaceTowardsDirection(initiator);
        if(dialog.Lines.Count > 0)
        {
            yield return GameManager.instance.GetDialogSystem.ShowDialogBox(dialog);
        }

        if(interactionWillNeverFight == true)
        {
            idleTimer = 0;
            if (lookDirectionAfterInteraction == true)
            {
                //FaceTowardsDirection(directionAfterInteraction);
            }
        }
        else
        {
            GameManager.instance.StartWildPokemonBattle(this);
        }
    }

    //[Serializable]
    //struct TrainerSaveData
    //{
    //    public bool lostToPlayer;
    //}

    //[Serializable]
    //struct StartingLevelTrainerSaveData
    //{
    //    public bool lostToPlayer;
    //    public int savedPosX;
    //    public int savedPosY;
    //    public FacingDirections savedDirection;
    //}
}
