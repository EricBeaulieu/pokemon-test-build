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

    float _idleTimer = 0f;
    float _idleTimerLimit = 0f;

    [Tooltip("This is the amount of time the NPC will sit and when finished they will move")]
    [SerializeField] float timeUntilMoveMin;
    [Tooltip("This is to add a random timer to the Idle amount time, if this is less then the min then there will be no random range timer")]
    [SerializeField] float timeUntilMoveMax;

    [SerializeField] GameObject exclamationMark;

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

        FaceTowardsDirection(Vector2.down + (Vector2)transform.position);
        _idleTimerLimit = SetNewIdleTimer();
        exclamationMark.SetActive(false);
        pokemonParty = GetComponent<PokemonParty>();

        if (trainerBase.TrainerName == "")
        {
            Debug.LogWarning("This trainer has no name", gameObject);
        }
        if(trainerBase.GetCharacterArt.GetFrontBattleSprite.Length <= 0)
        {
            Debug.LogWarning("This trainer has no front Sprite", trainerBase.GetCharacterArt);
        }
    }

    IEnumerator IInteractable.OnInteract(Vector2 initiator)
    {
        _playerSpotted = true;
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
        if(_playerSpotted == false)
        {
            _idleTimer += Time.deltaTime;

            if (_idleTimer >= _idleTimerLimit)
            {
                FaceTowardsDirection(TestDirectionF());
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

    FacingDirections TestDirectionF()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return FacingDirections.Up;
            case 1:
                return FacingDirections.Down;
            case 2:
                return FacingDirections.Right;
            default:
                return FacingDirections.Left;
        }
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
}
