using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PokemonParty))]
public class TrainerController : Entity,IInteractable,ISaveable
{
    [Header("Trainer Controller")]
    [SerializeField] TrainerBaseSO trainerBase;
    [SerializeField] SaveableEntity saveableEntity;
    bool hasLostToPlayer = false;
    
    public PokemonParty pokemonParty { get; private set; }

    [Range(1,7)]
    [SerializeField] int lineOfSightSize = 1;
    [SerializeField]BoxCollider2D lineofSight;
    bool _changingSight;
    const float BOX_STANDARD_SIZE = 0.25f;
    bool _playerSpotted = false;
    bool _interactWhenPossible;

    [SerializeField] GameObject exclamationMark;

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


    void Awake()
    {
        base.Initialization(trainerBase);

        if (GameManager.instance.startNewSaveEveryStart == false)
        {
            object previousSave = SavingSystem.ReturnSpecificSave(saveableEntity.GetID);
            if (previousSave != null)
            {
                saveableEntity.RestoreState(previousSave);
            }
        }

        FaceTowardsDirection(GlobalTools.GetDirectionFacingOnStart(this));
        _idleTimerLimit = SetNewIdleTimer();
        exclamationMark.SetActive(false);
        pokemonParty = GetComponent<PokemonParty>();

        aiDecisionList = CheckTrainerStartingDecisions(aiDecisionList);

        if (trainerBase.TrainerName == "")
        {
            Debug.LogWarning("This trainer has no name", gameObject);
        }
        if(trainerBase.GetCharacterArt.GetFrontBattleSprite.Length <= 0)
        {
            Debug.LogWarning("This trainer has no front Sprite", trainerBase.GetCharacterArt);
        }
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
        _playerSpotted = true;
        currentlyExecutingDecision = false;
        FaceTowardsDirection(initiator);

        if (hasLostToPlayer == false)
        {
            yield return GameManager.instance.GetDialogSystem.ShowDialogBox(trainerBase.GetPreBattleDialog);
            _idleTimer = 0;
            GameManager.instance.StartTrainerBattle(this);
        }
        else
        {
            yield return GameManager.instance.GetDialogSystem.ShowDialogBox(trainerBase.GetPostDefeatOverworldDialog);
        }
    }

    public override void HandleUpdate()
    {
        if(_playerSpotted == false && currentlyExecutingDecision == false)
        {
            _idleTimer += Time.deltaTime;

            if (_idleTimer >= _idleTimerLimit)
            {
                if(standAroundAndLook == true)
                {
                    FaceTowardsDirection(LookTowards());
                    _idleTimerLimit = SetNewIdleTimer();
                }
                else
                {
                    if (aiDecisionList.Count > 0)
                    {
                        if (aiDecisionList[_currentMovementPattern].movement != Vector2.zero)
                        {
                            StartCoroutine(Walk());
                        }
                        else if (aiDecisionList[_currentMovementPattern].directionToFace != Vector2.zero)
                        {
                            base.FaceTowardsDirection(aiDecisionList[_currentMovementPattern].directionToFace + (Vector2)transform.position);
                            _currentMovementPattern = (_currentMovementPattern + 1) % aiDecisionList.Count;
                        }

                        if (aiDecisionList[_currentMovementPattern].specificTimeUniltNextExecution > 0)
                        {
                            _idleTimerLimit = aiDecisionList[_currentMovementPattern].specificTimeUniltNextExecution;
                            _currentMovementPattern = (_currentMovementPattern + 1) % aiDecisionList.Count;
                        }
                        else
                        {
                            _idleTimerLimit = SetNewIdleTimer();
                        }
                    }
                }
                _idleTimer = 0;
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

    protected override void FaceTowardsDirection(FacingDirections dir)
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

    float SetNewIdleTimer()
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

    void OnTriggerEnter2D(Collider2D col)
    {
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

            _playerSpotted = true;
            currentlyExecutingDecision = false;
            StartCoroutine(TriggerTrainerBattle(player));
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
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

        yield return MoveToPosition(targetPos);

        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);

        player.LookAtTrainer(transform.position);
        yield return GameManager.instance.GetDialogSystem.ShowDialogBox(trainerBase.GetPreBattleDialog);

        _idleTimer = 0;
        GameManager.instance.StartTrainerBattle(this);
        player.spottedByTrainer = false;
    }

    public Sprite[] FrontBattleSprite
    {
        get { return trainerBase.GetCharacterArt.GetFrontBattleSprite; }
    }

    public string TrainerName
    {
        get { return trainerBase.TrainerName; }
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
            return trainerBase.GetInBattleDialogOnDefeat.Lines;
        }
        else
        {
            return trainerBase.GetInBattleDialogOnVictory.Lines;
        }
    }

    public object CaptureState(bool PlayerSave = false)
    {
        if(PlayerSave == true && gameObject.scene.name == SceneSystem.currentLevelManager.GameSceneBase.GetSceneName)
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

    public TrainerBaseSO GetTrainerBase
    {
        get { return trainerBase; }
    }

    List<AIDecision> CheckTrainerStartingDecisions(List<AIDecision> currentDecisions)
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

}
