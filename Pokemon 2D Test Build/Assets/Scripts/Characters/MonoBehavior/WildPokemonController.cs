using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildPokemonController : EntityAI,IInteractable
{
    [SerializeField] WildPokemon _pokemon;
    public WildPokemon pokemon { get { return _pokemon; } private set { _pokemon = value; } }
    [SerializeField] bool alwaysInOverworld = false;
    [SerializeField] bool legendary = false;
    [Tooltip("Will use the same feature as the trainer, it will have a line of sight and say its name and fight the player")]
    bool isAggressive = false;

    [Tooltip("Interactable but the player will never fight")]
    [SerializeField] bool interactionWillNeverFight = false;
    [SerializeField] Dialog dialog;
    [SerializeField] bool spawnInAnimation = true;

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

        if(spawnInAnimation == true)
        {
            StartCoroutine(SpawnIn());
        }
    }

    public override void HandleUpdate()
    {

    }

    public IEnumerator OnInteract(Vector2 initiator)
    {
        FaceTowardsDirection(initiator);
        if(dialog.Lines.Count > 0)
        {
            yield return GameManager.instance.GetDialogSystem.ShowDialogBox(dialog);
        }

        if(interactionWillNeverFight == true)
        {
            idleTimer = 0;
            if (lookDirectionAfterInteraction == true)
            {
                FaceTowardsDirection(directionAfterInteraction);
            }
        }
        else
        {
            GameManager.instance.StartWildPokemonBattle(this);
        }
    }

    IEnumerator SpawnIn()
    {
        Debug.Log("Spawning in Running");
        float duration = 0.75f;
        float minimalSize = 0.25f;
        float difference = 0.75f;
        graphics.localScale = new Vector3(minimalSize, minimalSize);
        SpriteRenderer spriteRender = graphics.GetComponentInChildren<SpriteRenderer>();
        spriteRender.color.SetDarkness(0);
        float totalTime = 0f;

        while (totalTime <= duration)
        {
            graphics.localScale = new Vector3(minimalSize + (difference * (totalTime / duration)), minimalSize + (difference * (totalTime / duration)));
            totalTime += Time.deltaTime;
            yield return null;
        }

        graphics.localScale = new Vector3(1, 1);
        totalTime = 0f;
        duration = 0.75f;


        while (totalTime <= duration)
        {
            spriteRender.color.SetDarkness(totalTime / duration);
            totalTime += Time.deltaTime;
            yield return null;
        }
    }

    public void SetWildPokemon(WildPokemon wildPokemon)
    {
        pokemon = wildPokemon;
        
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
