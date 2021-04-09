using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PokemonParty))]
public class PlayerController : Entity
{
    [SerializeField] string playerName;
    [SerializeField] Sprite[] playerBackSprite;

    public event Action OnEncounter;

    Vector2 _currentInput;

    public bool spottedByTrainer;
    bool _ignorePlayerInput;

    protected override void Awake()
    {
        base.Awake();


        //Debug

        if(playerName == "")
        {
            Debug.Log("Players Name is missing");
        }
        if(playerBackSprite.Length <=0)
        {
            Debug.LogWarning("Current Player is missing their battle back sprite Sheet");
        }

        _ignorePlayerInput = false;
    }

    public override void HandleUpdate()
    {
        if(spottedByTrainer == false && _ignorePlayerInput == false)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isRunning = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isRunning = false;
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
                    Interact();
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

        CheckForWildEncounter();
    }

    void CheckForWildEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.25f, grassLayermask) != null)
        {
            _anim.SetBool("isMoving", false);
            OnEncounter();
        }
    }

    void Interact()
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
                        StartCoroutine(InteractWhenPossible(currentInteractable));
                    }
                }
                else
                {
                    currentInteractable.GetComponent<IInteractable>()?.OnInteract((Vector2)transform.position);
                }
            }
            else
            {
                collider.GetComponent<IInteractable>()?.OnInteract((Vector2)transform.position);
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

        currentNPC.GetComponent<IInteractable>()?.OnInteract((Vector2)transform.position);
        _ignorePlayerInput = false;
    }

    public void LookAtTrainer(Vector2 trainerPos)
    {
        FaceTowardsDirection(trainerPos);
    }

    public Sprite[] BackBattleSprite
    {
        get { return playerBackSprite; }
    }

    public string TrainerName
    {
        get { return playerName; }
    }

    public void PlayerHasLostBattle()
    {
        transform.position = new Vector2(10.5f, .5f);
        Debug.Log("player Has Lost");
        //Go to pokemon center mechanic
        FaceTowardsDirection(FacingDirections.Down);
    }
}
