using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PokemonParty))]
public class PlayerController : Entity
{
    [Header("Player Controller")]
    [SerializeField] string trainerName;
    int trainerIDNumber;
    public PokemonParty pokemonParty { get; private set; }
    
    bool wildEncountersGrassSpecific;
    public event Action OpenStartMenu;
    public event Action <Portal> PortalEntered;
    event Action OnMoveOverFinished;

    Vector2 _currentInput;

    [Header("Player Audio")]
    [SerializeField] AudioSource audioSFX;
    [SerializeField] AudioClip bumpSFX;
    [SerializeField] AudioClip ledgeJumpSFX;
    [SerializeField] AudioClip itemObtainedSFX;
    
    public bool spottedByTrainer { get; set; }
    bool _ignorePlayerInput;
    bool _ignoreMenuOpen;

    TriggerableRegion lastTriggerableRegion;

    public int money { get; set; } = 100000;
    [SerializeField] GameObject exclamationMark;

    //Surfing
    const string surfingAvailable = "The water is dyed a deep blue...\nWould you like to surf?";
    bool animationActive = false;
    SurfableEntity surfableEntity;

    const int OLD_ROD_FISHING_PERCENT = 50;
    const int GOOD_ROD_FISHING_PERCENT = 70;
    const int SUPER_ROD_FISHING_PERCENT = 85;
    const int ROCK_SMASH_PERCENT = 40;

    void Awake()
    {
        base.Initialization();

        pokemonParty = GetComponent<PokemonParty>();
        //Debug

        if (string.IsNullOrEmpty(trainerName) == true)
        {
            Debug.LogWarning("This trainer has no name", gameObject);
        }

        if (CharacterArt.GetFrontBattleSprite.Length <= 0)
        {
            Debug.LogWarning("This trainer has no front Sprite", CharacterArt);
        }

        if (CharacterArt.GetBackBattleSprite.Length <=0)
        {
            Debug.LogWarning("Current Player is missing their battle back sprite Sheet");
        }
        
        trainerIDNumber = UnityEngine.Random.Range(0, 99999);

        _ignorePlayerInput = false;
        _ignoreMenuOpen = false;
    }

    public override void HandleUpdate()
    {
        if(spottedByTrainer == false && _ignorePlayerInput == false)
        {
            if(_ignoreMenuOpen == false)
            {
                if (Input.GetButtonDown("Cancel") && IsMoving == false && isRunning == false)
                {
                    OpenStartMenu();
                    _ignorePlayerInput = true;
                }
            }
            else
            {
                _ignoreMenuOpen = false;
            }


            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(isBiking == false || isSurfing == false)
                {
                    isRunning = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if(isRunning)
                {
                    isRunning = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Alpha0))
            {
                isBiking = !isBiking;
            }

            if (Input.GetKeyUp(KeyCode.Alpha9))
            {
                GoFishing();
            }

            if (Input.GetKeyUp(KeyCode.Alpha8))
            {
                GoFishing(false);
            }

            if (Input.GetKeyUp(KeyCode.Alpha7))
            {
                GoFishing(true);
            }

            if (IsMoving == false)
            {
                _currentInput.x = Input.GetAxisRaw("Horizontal");
                _currentInput.y = Input.GetAxisRaw("Vertical");

                //For Xbox controller since raw doesnt work and will still give range values for some reason
                //Also to clamp the NPC incase theyre value is above 1
                if(_currentInput.x != 0)
                {
                    _currentInput.x = _currentInput.x > 0 ? 1 : -1;
                }

                if(_currentInput.y != 0)
                {
                    _currentInput.y = _currentInput.y > 0 ? 1 : -1;
                }

                //Removes Diagnol Input
                if (_currentInput.x != 0) _currentInput.y = 0;

                if (_currentInput != Vector2.zero)
                {
                    StartCoroutine(MoveToPosition(_currentInput));
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    StartCoroutine(Interact());
                    isRunning = false;
                }
            }
        }
        _anim.SetBool("isMoving", IsMoving);
        _anim.SetBool("isRunning", isRunning);
        _anim.SetBool("isBiking", isBiking);

        if(surfableEntity != null)
        {
            surfableEntity.SnaptoDirection(GlobalTools.CurrentDirectionFacing(_anim));
        }
    }

    protected override IEnumerator MoveToPosition(Vector2 moveVector)
    {
        Vector2 currentPos = transform.position;
        yield return base.MoveToPosition(moveVector);

        if((Vector2)transform.position != currentPos)
        {
            OnMoveOver();
        }
    }

    void OnMoveOver()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.25f, grassLayermask| waterLayerMask | portalLayerMask|triggerLayerMask);

        TriggerableRegion triggerable = null;

        if (col != null)
        {
            if(grassLayermask == (grassLayermask | (1 << col.gameObject.layer)))
            {
                PlayerHasWildEncounter(WildPokemonEncounterTypes.Walking);
            }
            else if (waterLayerMask == (waterLayerMask | (1 << col.gameObject.layer)))
            {
                PlayerHasWildEncounter(WildPokemonEncounterTypes.Surfing);
            }
            else if(portalLayerMask == (portalLayerMask | (1 << col.gameObject.layer)))
            {
                _anim.SetBool("isMoving", false);
                isRunning = false;
                PortalEntered(col.GetComponent<Portal>());
                return;
            }
            else if(triggerLayerMask == (triggerLayerMask | (1 << col.gameObject.layer)))
            {
                triggerable = col.GetComponent<TriggerableRegion>();
                if(triggerable != null && triggerable != lastTriggerableRegion)
                {
                    triggerable.TriggerEntered();
                    lastTriggerableRegion = triggerable;
                    return;
                }
            }
        }
        else
        {
            lastTriggerableRegion = null;
        }


        if (wildEncountersGrassSpecific == false)
        {
            PlayerHasWildEncounter(WildPokemonEncounterTypes.Walking);
        }

        if(OnMoveOverFinished != null)
        {
            OnMoveOverFinished.Invoke();
            OnMoveOverFinished = null;
        }
    }

    IEnumerator Interact()
    {
        Vector2 interactablePOS = (Vector2)transform.position + new Vector2(_anim.GetFloat("moveX"), _anim.GetFloat("moveY"));

        Debug.DrawLine(transform.position, interactablePOS,Color.red,1f);
        Collider2D interactor = Physics2D.OverlapCircle(interactablePOS, 0.25f, interactableLayermask);

        if (interactor != null)
        {
            if(interactor.transform.root.GetComponent<Entity>() == true)
            {
                Entity currentInteractable = interactor.transform.root.GetComponent<Entity>();
                if (currentInteractable.IsMoving == true)
                {
                    if(interactablePOS == currentInteractable.CurrentWalkingToPos())
                    {
                        _ignorePlayerInput = true;
                        yield return InteractWhenPossible(currentInteractable);
                    }
                }
                else
                {
                    yield return currentInteractable.GetComponent<IInteractable>()?.OnInteract((Vector2)transform.position);
                }
            }
            else
            {
                yield return interactor.GetComponent<IInteractable>()?.OnInteract((Vector2)transform.position);
            }
        }

        Collider2D col = Physics2D.OverlapCircle(interactablePOS, 0.25f, waterLayerMask);

        if(col != null && isSurfing == false)
        {
            yield return TryToSurf(interactablePOS);
        }
    }

    IEnumerator InteractWhenPossible(Entity currentNPC)
    {
        currentNPC.PlayerInteractingWithWhenDoneMoving();

        while (currentNPC.IsMoving == true)
        {
            yield return null;
        }

        yield return currentNPC.GetComponent<IInteractable>()?.OnInteract((Vector2)transform.position);
        _ignorePlayerInput = false;
    }

    public void LookAtTrainer(Vector2 trainerPos)
    {
        FaceTowardsDirection(trainerPos);
        isRunning = false;
    }

    public Sprite[] BackBattleSprite
    {
        get { return CharacterArt.GetBackBattleSprite; }
    }

    public string TrainerName
    {
        get { return trainerName; }
    }

    public string TrainerIDNumber
    {
        get { return trainerIDNumber.ToString("00000"); }
    }

    public void PlayerHasLostBattle()
    {
        transform.position = new Vector2(10.5f, .5f);
        Debug.Log("player Has Lost");
        //Go to pokemon center mechanic
        pokemonParty.HealAllPokemonInParty();
        FaceTowardsDirection(FacingDirections.Down);
    }

    public void ReturnFromStartMenu()
    {
        _ignorePlayerInput = false;
        _ignoreMenuOpen = true;
    }

    #region PokemonBattleEncounters
    public void SetWildEncounter(bool grassSpecific)
    {
        wildEncountersGrassSpecific = grassSpecific;
    }

    void PlayerHasWildEncounter(WildPokemonEncounterTypes encounterType)
    {
        if(GameManager.instance.turnOffWildPokemon == true)
        {
            return;
        }

        if(encounterType == WildPokemonEncounterTypes.Walking || encounterType == WildPokemonEncounterTypes.Surfing)
        {
            if (Random.Range(1, 101) > 11)
            {
                return;
            }
        }

        _anim.SetBool("isMoving", false);
        isRunning = false;
        GameManager.instance.StartWildPokemonBattle(encounterType);


    }

    #endregion

    #region PlayerSaveData
    public object CaptureState()
    {
        isRunning = false;
        return new PlayerSaveData
        {
            playerPosX = Mathf.FloorToInt(transform.position.x),
            playerPosY = Mathf.FloorToInt(transform.position.y),
            trainerName = TrainerName,
            savedParty = pokemonParty.CurrentPokemonList().Select(x => x.GetSaveData()).ToList(),
            savedDirection = GlobalTools.CurrentDirectionFacing(_anim)
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (PlayerSaveData)state;
        SnapToGrid();
        trainerName = saveData.trainerName;
        pokemonParty.LoadPlayerParty(saveData.savedParty.Select(x => new Pokemon(x)).ToList());
        FaceTowardsDirection(saveData.savedDirection);
    }

    #endregion

    public void PlaySFX(AudioClip clip, float volume = 1)
    {
        audioSFX.clip = clip;
        audioSFX.volume = volume;
        audioSFX.Play();
    }

    public IEnumerator PlayHMAnimation(Pokemon pokemonUsingHm)
    {
        yield return PlayAnimatorAnimation("HMStart");
        yield return GameManager.instance.PlayerUsedHMAnimation(pokemonUsingHm);
        yield return PlayAnimatorAnimation("HMEnd");
    }

    IEnumerator PlayAnimatorAnimation(string TriggerName = null)
    {
        if (string.IsNullOrEmpty(TriggerName) == true)
        {
            yield break;
        }

        animationActive = true;
        _anim.SetTrigger(TriggerName);

        while (animationActive == true)
        {
            yield return null;
        }
    }

    public void AnimationComplete()
    {
        animationActive = false;
    }

    IEnumerator PlayerCantUseThat()
    {
        DialogManager dialogManager = GameManager.instance.GetDialogSystem;
        dialogManager.ActivateDialog(true);
        yield return dialogManager.TypeDialog($"{trainerName}! This isn't the time to use that!", true);
        dialogManager.ActivateDialog(false);
    }

    /// <summary>
    /// When an item is used the player will check to see if there is water infront of them and play the fishing animation
    /// </summary>
    /// <param name="rod">
    /// null == old rod
    /// false == good rod
    /// true == super rod
    /// </param>
    public void GoFishing(bool? rod = null)
    {
        Vector2 interactablePOS = (Vector2)transform.position + new Vector2(_anim.GetFloat("moveX"), _anim.GetFloat("moveY"));
        Collider2D col = Physics2D.OverlapCircle(interactablePOS, 0.25f, waterLayerMask);

        if(col != null)
        {
            Debug.Log("called coroutine");
            StartCoroutine(GoFishingCoroutine(rod));
        }
        else
        {
            StartCoroutine(PlayerCantUseThat());
        }
    }

    IEnumerator GoFishingCoroutine(bool? rod = null)
    {
        yield return PlayAnimatorAnimation("GoFishing");
        int catchRate = Random.Range(1, 101);

        if(rod == null)
        {
            if (catchRate <= OLD_ROD_FISHING_PERCENT)
            {
                catchRate = Random.Range(1, 10);
            }
        }
        else
        {
            if(rod == false)
            {
                if (catchRate <= GOOD_ROD_FISHING_PERCENT)
                {
                    catchRate = Random.Range(1, 10);
                }
            }
            else
            {
                if (catchRate <= SUPER_ROD_FISHING_PERCENT)
                {
                    catchRate = Random.Range(1, 10);
                }
            }
        }

        bool willCatchOnWait = true;

        if (catchRate >= 10)
        {
            willCatchOnWait = false;
            catchRate = 10;
        }

        bool done = false;
        float timer = 0;
        while (!done)//this is so the player can break out of fishing
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                willCatchOnWait = false;
                done = true; // breaks the loop
            }

            if (timer > catchRate)
            {
                done = true;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        DialogManager dialogManager = GameManager.instance.GetDialogSystem;

        if (willCatchOnWait == false)
        {
            yield return PlayAnimatorAnimation("Reel");
            dialogManager.ActivateDialog(true);
            yield return dialogManager.TypeDialog("Nothings Biting", true);
            dialogManager.ActivateDialog(false);
            yield break;
        }
        else
        {
            exclamationMark.SetActive(true);
            _anim.SetTrigger("Hooked");
            dialogManager.ActivateDialog(true);
            yield return dialogManager.TypeDialog("Somethings on the hook", true);
            exclamationMark.SetActive(false);
            dialogManager.ActivateDialog(false);
            yield return PlayAnimatorAnimation("Reel");

            if (rod == null)
            {
                PlayerHasWildEncounter(WildPokemonEncounterTypes.OldRod);
            }
            else
            {
                if (rod == false)
                {
                    PlayerHasWildEncounter(WildPokemonEncounterTypes.GoodRod);
                }
                else
                {
                    PlayerHasWildEncounter(WildPokemonEncounterTypes.SuperRod);
                }
            }
        }
    }

    IEnumerator TryToSurf(Vector2 currentPosition)
    {
        Pokemon pokemon = pokemonParty.ContainsMove("Surf");

        if (pokemon != null)
        {
            DialogManager dialogManager = GameManager.instance.GetDialogSystem;
            bool surfUsed = false;
            string pokemonUse = $"{pokemon.currentName} used surf!";
            dialogManager.ActivateDialog(true);
            yield return dialogManager.TypeDialog(surfingAvailable, true);
            yield return dialogManager.SetChoiceBox(() =>
            {
                surfUsed = true;
            });

            if (surfUsed == true)
            {
                yield return dialogManager.TypeDialog(pokemonUse);
                dialogManager.ActivateDialog(false);
                GameManager.SetGameState(GameState.Dialog);
                yield return PlayHMAnimation(pokemon);
                isSurfing = true;
                IsJumping = true;
                //Setting the surfable pokemon
                surfableEntity = GameManager.instance.GetPlayerSurfableEntityController;
                surfableEntity.gameObject.SetActive(true);
                surfableEntity.SetPositionPlusOffset(currentPosition, GlobalTools.CurrentDirectionFacing(_anim));
                yield return MoveToPosition(new Vector2(_anim.GetFloat("moveX"), _anim.GetFloat("moveY")));
                _anim.SetBool("isSurfing", true);
                GameManager.SetGameState(GameState.Overworld);
                surfableEntity.transform.parent = transform;
                surfableEntity.SetCharactersSpriteSwap(GetComponentInChildren<SpriteSheetSwap>(), GlobalTools.CurrentDirectionFacing(_anim));
            }
            else
            {
                dialogManager.ActivateDialog(false);
            }
        }
    }

    protected override void SurfingEnds()
    {
        base.SurfingEnds();
        surfableEntity.SetCharactersSpriteSwap();
        surfableEntity.transform.parent = null;
        OnMoveOverFinished += ClearSurfableEntity;
    }

    void ClearSurfableEntity()
    {
        surfableEntity.gameObject.transform.position = Vector3.zero;
        surfableEntity.gameObject.SetActive(false);
        surfableEntity = null;
        Debug.Log("SurfableEntityRemoved");
    }
}
