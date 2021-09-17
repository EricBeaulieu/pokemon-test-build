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
    [SerializeField] TrainerBaseSO trainerBase;
    string trainerName;
    int trainerIDNumber;
    public PokemonParty pokemonParty { get; private set; }
    
    bool wildEncountersGrassSpecific;
    public event Action OpenStartMenu;
    public event Action <Portal> PortalEntered;

    Vector2 _currentInput;

    [HideInInspector]
    public bool spottedByTrainer;
    bool _ignorePlayerInput;
    bool _ignoreMenuOpen;

    void Awake()
    {
        base.Initialization(trainerBase);

        pokemonParty = GetComponent<PokemonParty>();
        //Debug

        if(trainerBase.TrainerName == "")
        {
            Debug.Log("Players Name is missing");
        }
        if(trainerBase.GetCharacterArt.GetBackBattleSprite.Length <=0)
        {
            Debug.LogWarning("Current Player is missing their battle back sprite Sheet");
        }

        trainerName = trainerBase.TrainerName;
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
                isRunning = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isRunning = false;
            }

            if (Input.GetKeyUp(KeyCode.Alpha0))
            {
                GameManager.instance.LoadGame();
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
    }

    protected override IEnumerator MoveToPosition(Vector2 moveVector)
    {
        yield return base.MoveToPosition(moveVector);

        OnMoveOver();
    }

    void OnMoveOver()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.25f, grassLayermask|portalLayerMask);
        if(col != null)
        {
            if(grassLayermask == (grassLayermask | (1 << col.gameObject.layer)))
            {
                PlayerHasWildEncounter();
            }
            else if(portalLayerMask == (portalLayerMask | (1 << col.gameObject.layer)))
            {
                _anim.SetBool("isMoving", false);
                isRunning = false;
                PortalEntered(col.GetComponent<Portal>());
                return;
            }
        }

        if (wildEncountersGrassSpecific == false)
        {
            PlayerHasWildEncounter();
        }
    }

    IEnumerator Interact()
    {
        Vector2 facingDirection = new Vector2(_anim.GetFloat("moveX"), _anim.GetFloat("moveY"));
        Vector2 interactablePOS = (Vector2)transform.position + facingDirection;

        Debug.DrawLine(transform.position, interactablePOS,Color.red,1f);

        var collider = Physics2D.OverlapCircle(interactablePOS, 0.25f, interactableLayermask);

        if (collider != null)
        {
            if(collider.transform.root.GetComponent<Entity>() == true)
            {
                Entity currentInteractable = collider.transform.root.GetComponent<Entity>();
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
                yield return collider.GetComponent<IInteractable>()?.OnInteract((Vector2)transform.position);
            }
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
        get { return trainerBase.GetCharacterArt.GetBackBattleSprite; }
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

    public void SetWildEncounter(bool grassSpecific)
    {
        wildEncountersGrassSpecific = grassSpecific;
    }

    void PlayerHasWildEncounter()
    {
        if(Random.Range(1,101) <= 11)
        {
            _anim.SetBool("isMoving", false);
            isRunning = false;
            GameManager.instance.StartWildPokemonBattle();
        }
    }

    public object CaptureState()
    {
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
   
}
