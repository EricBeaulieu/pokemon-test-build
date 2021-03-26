using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PokemonParty))]
public class TrainerController : Entity,IInteractable
{
    [SerializeField] Dialog preBattleDialog;
    bool _hasLostToPlayer = false;
    [SerializeField] Dialog postDefeatDialog;

    [SerializeField] string trainerName;
    [SerializeField] Sprite[] frontBattleSprite;
    PokemonParty pokemonParty;

    [Range(1,7)]
    [SerializeField] int lineOfSightSize = 1;
    [SerializeField]BoxCollider2D lineofSight;
    const float BOX_STANDARD_SIZE = 0.25f;
    bool _playerSpotted = false;

    float _idleTimer = 0f;
    float _idleTimerLimit = 0f;

    [Tooltip("This is the amount of tim e the NPC will sit and when finished they will move")]
    [SerializeField] float timeUntilMoveMin;
    [Tooltip("This is to add a random timer to the Idle amount time, if this is less then the min then there will be no random range timer")]
    [SerializeField] float timeUntilMoveMax;

    [SerializeField] GameObject exclamationMark;

    protected override void Awake()
    {
        base.Awake();
        FaceTowardsDirection(Vector2.down + (Vector2)transform.position);
        _idleTimerLimit = SetNewIdleTimer();
        exclamationMark.SetActive(false);

        if(trainerName == "")
        {
            Debug.LogWarning("This trainer has no name", gameObject);
        }
        if(frontBattleSprite.Length <= 0)
        {
            Debug.LogWarning("This trainer has no front Sprite", gameObject);
        }
    }

    void IInteractable.OnInteract(Vector2 initiator)
    {
        _playerSpotted = true;
        FaceTowardsDirection(initiator);

        if (_hasLostToPlayer == false)
        {
            StartCoroutine(DialogManager.instance.ShowDialogBox(preBattleDialog, () =>
            {
                _idleTimer = 0;
                GameManager.instance.StartTrainerBattle(this);
            }));
        }
        else
        {
            StartCoroutine(DialogManager.instance.ShowDialogBox(postDefeatDialog));
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

        _anim.SetBool("isMoving", _isMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    protected override void FaceTowardsDirection(Vector2 targetPos)
    {
        base.FaceTowardsDirection(targetPos);

        AdjustSight(targetPos);
    }

    protected override void FaceTowardsDirection(FacingDirections dir)
    {
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
        if(col.CompareTag("Player")&& _playerSpotted == false && _hasLostToPlayer == false)
        {
            _playerSpotted = true;
            PlayerController player = col.GetComponent<PlayerController>();
            StartCoroutine(TriggerTrainerBattle((Vector2)col.transform.position,player));
        }
    }

    public IEnumerator TriggerTrainerBattle(Vector2 playerPos,PlayerController player)
    {
        player.spottedByTrainer = true;
        exclamationMark.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        exclamationMark.SetActive(false);

        //Walk toward player
        Vector2 targetPos = playerPos - (Vector2)transform.position;
        targetPos -= targetPos.normalized;
        targetPos = new Vector2(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y));

        yield return MoveToPosition(targetPos);

        _anim.SetBool("isMoving", _isMoving);
        _anim.SetBool("isRunning", isRunning);

        player.LookAtTrainer(transform.position);
        yield return DialogManager.instance.ShowDialogBox(preBattleDialog, () =>
        {
            _idleTimer = 0;
            GameManager.instance.StartTrainerBattle(this);
        });

        player.spottedByTrainer = false;
    }

    public void HasLostBattleToPlayer()
    {
        _hasLostToPlayer = true;
        _playerSpotted = false;
    }

    public Sprite[] FrontBattleSprite
    {
        get { return frontBattleSprite; }
    }

    public string TrainerName
    {
        get { return trainerName; }
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

}
