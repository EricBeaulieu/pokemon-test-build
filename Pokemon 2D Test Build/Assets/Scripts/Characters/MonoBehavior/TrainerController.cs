using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PokemonParty))]
public class TrainerController : Entity,IInteractable
{
    [Header("Trainer Controller")]
    [SerializeField] TrainerBaseSO trainerBase;
    int uniqueID;
    bool _hasLostToPlayer = false;
    
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

        uniqueID = trainerBase.GetInstanceID();

        if(GameManager.instance.startNewSaveEveryStart == false)
        {
            _hasLostToPlayer = SavingSystem.GetTrainerSave(uniqueID);
        }

        FaceTowardsDirection(Vector2.down + (Vector2)transform.position);
        _idleTimerLimit = SetNewIdleTimer();
        exclamationMark.SetActive(false);
        pokemonParty = GetComponent<PokemonParty>();

        if (trainerBase == null)
        {
            Debug.LogWarning("This trainer has no Data Entered", gameObject);
            return;
        }

        if (trainerBase.TrainerName == "")
        {
            Debug.LogWarning("This trainer has no name", gameObject);
        }
        if(trainerBase.GetCharacterArt.GetFrontBattleSprite.Length <= 0)
        {
            Debug.LogWarning("This trainer has no front Sprite", trainerBase.GetCharacterArt);
        }
    }

    void IInteractable.OnInteract(Vector2 initiator)
    {
        _playerSpotted = true;
        FaceTowardsDirection(initiator);

        if (_hasLostToPlayer == false)
        {
            StartCoroutine(GameManager.instance.GetDialogSystem.ShowDialogBox(trainerBase.GetPreBattleDialog, () =>
            {
                _idleTimer = 0;
                GameManager.instance.StartTrainerBattle(this);
            }));
        }
        else
        {
            StartCoroutine(GameManager.instance.GetDialogSystem.ShowDialogBox(trainerBase.GetPostDefeatOverworldDialog));
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
        if(col.CompareTag("Player")&& _playerSpotted == false && _hasLostToPlayer == false && _changingSight == false)
        {
            _playerSpotted = true;
            PlayerController player = col.GetComponent<PlayerController>();
            StartCoroutine(TriggerTrainerBattle(player));
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
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
        yield return GameManager.instance.GetDialogSystem.ShowDialogBox(trainerBase.GetPreBattleDialog, () =>
        {
            _idleTimer = 0;
            GameManager.instance.StartTrainerBattle(this);
        });

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

    Vector2 TestDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return Vector2.up + (Vector2)transform.position;
            case 1:
                return Vector2.right + (Vector2)transform.position;
            case 2:
                return Vector2.down + (Vector2)transform.position;
            case 3:
                return Vector2.left + (Vector2)transform.position;
            default:
                Debug.Log("This is going higher then expected and breaking");
                break;
        }

        return Vector2.zero + (Vector2)transform.position;
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
            case 3:
                return FacingDirections.Left;
            default:
                Debug.Log("This is going higher then expected and breaking");
                break;
        }

        return FacingDirections.Down;
    }

    /// <summary>
    /// This sets the players dialog as well as sets all the variables upon defeat
    /// </summary>
    /// <param name="trainerHasWon">If this trainer has won the battle or not</param>
    /// <returns></returns>
    public List<string> OnBattleOverDialog(bool playerHasWon)
    {
        _hasLostToPlayer = playerHasWon;
        _playerSpotted = false;

        if (playerHasWon == true)
        {
            SavingSystem.AddDefeatedTrainerToStack(uniqueID);
            return trainerBase.GetInBattleDialogOnDefeat.Lines;
        }
        else
        {
            return trainerBase.GetInBattleDialogOnVictory.Lines;
        }
    }
}
