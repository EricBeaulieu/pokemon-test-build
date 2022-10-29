using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PokemonParty))]
public class TrainerController : EntityAI,IInteractable,ISaveable
{
    [Header("Trainer Controller")]
    [SerializeField] string trainerName;
    [SerializeField] Dialog preBattleDialog;
    [SerializeField] Dialog inBattleDialogOnDefeat;
    [SerializeField] Dialog inBattleDialogOnVictory;
    [SerializeField] Dialog postDefeatOverworldDialog;
    [SerializeField] int payoutUponDefeat;
    public int PayoutUponDefeat { get { return payoutUponDefeat; } }
    
    [SerializeField] SaveableEntity saveableEntity;
    bool hasLostToPlayer = false;
    
    public PokemonParty pokemonParty { get; private set; }

    [Range(1,7)]
    [SerializeField] int lineOfSightSize = 1;
    [SerializeField] BoxCollider2D lineofSight;
    [SerializeField] AudioClip specializedBattleMusic;
    public AudioClip getBattleMusic { get { return specializedBattleMusic; } }
    bool _changingSight;
    const float BOX_STANDARD_SIZE = 0.25f;
    bool _playerSpotted = false;

    [SerializeField] GameObject exclamationMark;

    void Awake()
    {
        base.Initialization();

        if (GameManager.instance.startNewSaveEveryStart == false)
        {
            object previousSave = SavingSystem.ReturnSpecificSave(saveableEntity.GetID);
            if (previousSave != null)
            {
                saveableEntity.RestoreState(previousSave);
            }
        }

        FaceTowardsDirection(GlobalTools.GetDirectionFacingOnStart(this));
        idleTimerLimit = SetNewIdleTimer();
        exclamationMark.SetActive(false);
        pokemonParty = GetComponent<PokemonParty>();

        aiDecisionList = CheckAIDecisions(aiDecisionList);

        if (string.IsNullOrEmpty(trainerName) == true)
        {
            Debug.LogWarning("This trainer has no name", gameObject);
        }

        if(CharacterArt.GetFrontBattleSprite.Length <= 0)
        {
            Debug.LogWarning("This trainer has no front Sprite", CharacterArt);
        }

        if (preBattleDialog.Lines.Count <= 0)
        {
            Debug.LogError("This trainer is missing its preBattleDialog", gameObject);
        }

        if (inBattleDialogOnDefeat.Lines.Count <= 0)
        {
            Debug.LogError("This trainer is missing its inBattleDialogOnDefeat", gameObject);
        }

        if (inBattleDialogOnVictory.Lines.Count <= 0)
        {
            Debug.LogError("This trainer is missing its inBattleDialogOnVictory", gameObject);
        }

        if (postDefeatOverworldDialog.Lines.Count <= 0)
        {
            Debug.LogError("This trainer is missing its postDefeatOverworldDialog", gameObject);
        }
    }

    public IEnumerator OnInteract(Vector2 initiator)
    {
        _playerSpotted = true;
        currentlyExecutingDecision = false;
        FaceTowardsDirection(initiator);

        if (hasLostToPlayer == false)
        {
            yield return GameManager.instance.GetDialogSystem.ShowDialogBox(preBattleDialog);
            idleTimer = 0;
            GameManager.instance.StartTrainerBattle(this);
        }
        else
        {
            yield return GameManager.instance.GetDialogSystem.ShowDialogBox(postDefeatOverworldDialog);
        }
    }

    public override void HandleUpdate()
    {
        if(_playerSpotted == false && currentlyExecutingDecision == false)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTimerLimit)
            {
                if(standAroundAndLook == true)
                {
                    FaceTowardsDirection(LookTowards());
                    idleTimerLimit = SetNewIdleTimer();
                }
                else
                {
                    if (aiDecisionList.Count > 0)
                    {
                        if (aiDecisionList[currentMovementPattern].movement != Vector2.zero)
                        {
                            StartCoroutine(Walk());
                        }
                        else if (aiDecisionList[currentMovementPattern].directionToFace != Vector2.zero)
                        {
                            base.FaceTowardsDirection(aiDecisionList[currentMovementPattern].directionToFace + (Vector2)transform.position);
                            currentMovementPattern = (currentMovementPattern + 1) % aiDecisionList.Count;
                        }

                        if (aiDecisionList[currentMovementPattern].specificTimeUniltNextExecution > 0)
                        {
                            idleTimerLimit = aiDecisionList[currentMovementPattern].specificTimeUniltNextExecution;
                            currentMovementPattern = (currentMovementPattern + 1) % aiDecisionList.Count;
                        }
                        else
                        {
                            idleTimerLimit = SetNewIdleTimer();
                        }
                    }
                }
                idleTimer = 0;
            }
        }

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    protected override void FaceTowardsDirection(Vector2 targetPos)
    {
        _changingSight = true;
        base.FaceTowardsDirection(targetPos);

        AdjustSight(targetPos);
    }

    public override void FaceTowardsDirection(FacingDirections dir)
    {
        _changingSight = true;
        base.FaceTowardsDirection(dir);

        Vector2 targetDir = new Vector2().GetDirection(dir);
        targetDir += (Vector2)transform.position;

        AdjustSight(targetDir);
    }

    void AdjustSight(Vector2 direction)
    {
        if(direction == Vector2.zero)
        {
            return;
        }

        Vector2 dirToFace = direction - (Vector2)transform.position;
        dirToFace = dirToFace.normalized;

        lineofSight.offset = (((dirToFace * lineOfSightSize) - dirToFace)/2) + dirToFace;

        float Xsize = ((Mathf.Abs(dirToFace.x) * lineOfSightSize) - Mathf.Abs(dirToFace.x)) + BOX_STANDARD_SIZE;
        float Ysize = ((Mathf.Abs(dirToFace.y) * lineOfSightSize) - Mathf.Abs(dirToFace.y)) + BOX_STANDARD_SIZE;

        lineofSight.size = new Vector2(Xsize, Ysize);
        _changingSight = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(GameManager.instance.turnOffTrainerBattles == true)
        {
            return;
        }

        if(col.CompareTag("Player")&& _playerSpotted == false && hasLostToPlayer == false && _changingSight == false)
        {
            PlayerController player = col.GetComponent<PlayerController>();

            Vector2 playerFinalPos = player.CurrentWalkingToPos() - (Vector2)transform.position;
            playerFinalPos.Normalize();
            if (Mathf.Abs(playerFinalPos.x) != 1 && Mathf.Abs(playerFinalPos.y) != 1)
            {
                Debug.Log($"Player position moving to {playerFinalPos}, Trainer current Position {transform.position}, normalized vecotr {playerFinalPos}");
                return;
            }

            int distance = Mathf.RoundToInt(Vector2.Distance(transform.position, player.transform.position));
            Vector2 directionFacing = GetDirection();
            for (int i = 1; i < distance; i++)
            {
                if (CheckIfWalkable(directionFacing * i) == false)
                {
                    positionMovingTo.localPosition = Vector2.zero;
                    return;
                }
            }
            
            _playerSpotted = true;
            currentlyExecutingDecision = false;
            StartCoroutine(TriggerTrainerBattle(player,distance));
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player,int distance)
    {
        if(player.IsMoving == true)
        {
            yield return null;
        }
        
        player.spottedByTrainer = true;
        exclamationMark.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        exclamationMark.SetActive(false);

        while(player.IsMoving == true)
        {
            yield return null;
        }

        //Walk toward player
        Vector2 playerPos = player.transform.position;
        Vector2 targetPos = playerPos - (Vector2)transform.position;
        targetPos -= targetPos.normalized;
        targetPos = new Vector2(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y));

        for (int i = 0; i < distance; i++)
        {
            yield return MoveToPosition(targetPos.normalized);
        }

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);

        player.LookAtTrainer(transform.position);
        yield return GameManager.instance.GetDialogSystem.ShowDialogBox(preBattleDialog);

        idleTimer = 0;
        GameManager.instance.StartTrainerBattle(this);
        player.spottedByTrainer = false;
    }

    public Sprite[] FrontBattleSprite
    {
        get { return CharacterArt.GetFrontBattleSprite; }
    }

    public string TrainerName
    {
        get { return trainerName; }
    }

    /// <summary>
    /// This sets the players dialog as well as sets all the variables upon defeat
    /// </summary>
    /// <param name="trainerHasWon">If this trainer has won the battle or not</param>
    /// <returns></returns>
    public List<string> OnBattleOverDialog(bool playerHasWon)
    {
        hasLostToPlayer = playerHasWon;
        _playerSpotted = false;

        if (playerHasWon == true)
        {
            SavingSystem.AddInfoTobeSaved(saveableEntity);
            return inBattleDialogOnDefeat.Lines;
        }
        else
        {
            return inBattleDialogOnVictory.Lines;
        }
    }

    public object CaptureState(bool PlayerSave = false)
    {
        if(PlayerSave == true && gameObject.scene.name == SceneSystem.currentLevelManager.GameSceneBase.name)
        {
            return new StartingLevelTrainerSaveData
            {
                lostToPlayer = hasLostToPlayer,
                savedPosX = Mathf.FloorToInt(transform.position.x),
                savedPosY = Mathf.FloorToInt(transform.position.y),
                savedDirection = GlobalTools.CurrentDirectionFacing(_anim)
            };
        }
        else
        {
            return new TrainerSaveData
            {
                lostToPlayer = hasLostToPlayer
            };
        }
    }

    public void RestoreState(object state)
    {
        if(state is TrainerSaveData)
        {
            var saveData = (TrainerSaveData)state;
            hasLostToPlayer = saveData.lostToPlayer;
        }
        else
        {
            var saveData = (StartingLevelTrainerSaveData)state;
            hasLostToPlayer = saveData.lostToPlayer;
            transform.position = new Vector2(saveData.savedPosX,saveData.savedPosY);
            SnapToGrid();
            FaceTowardsDirection(saveData.savedDirection);

            SavingSystem.AddInfoTobeSaved(saveableEntity);
        }
    }

    [Serializable]
    struct TrainerSaveData
    {
        public bool lostToPlayer;
    }

    [Serializable]
    struct StartingLevelTrainerSaveData
    {
        public bool lostToPlayer;
        public int savedPosX;
        public int savedPosY;
        public FacingDirections savedDirection;
    }
}
