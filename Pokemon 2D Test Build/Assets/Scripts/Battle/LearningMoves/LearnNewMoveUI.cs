using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LearnNewMoveUI : CoreSystem
{
    DialogManager dialogSystem;
    InventorySystem inventorySystem;
    [SerializeField] Image pokemonSprite;
    [SerializeField] Text pokemonName;
    [SerializeField] Image gender;
    [SerializeField] Image type1;
    [SerializeField] Image type2;

    [SerializeField] GameObject[] moveButton;

    bool playerInMenuMakingDecision;
    bool playerDoesNotWantToLearnMove = false;
    Move previousMove;

    public override void Initialization()
    {
        dialogSystem = GameManager.instance.GetDialogSystem;
        inventorySystem = GameManager.instance.GetInventorySystem;
        gameObject.SetActive(false);
    }

    public override void HandleUpdate()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RefuseToLearnMove();
        }
    }

    public IEnumerator PokemonWantsToLearnNewMoves(Pokemon pokemon, List<LearnableMove> movesToBeLearned)
    {
        foreach (LearnableMove learnableMove in movesToBeLearned)
        {
            if (pokemon.moves.Count < PokemonBase.MAX_NUMBER_OF_MOVES)
            {
                pokemon.LearnMove(learnableMove.moveBase);
                yield return dialogSystem.TypeDialog($"{pokemon.currentName} learned {learnableMove.moveBase.MoveName}!", true);
            }
            else
            {

                bool playerSelection = true;

                while (playerSelection == true)
                {
                    yield return dialogSystem.TypeDialog($"{pokemon.currentName} is trying to learn {learnableMove.moveBase.MoveName}.", true);
                    yield return dialogSystem.TypeDialog($"But {pokemon.currentName} can't learn more than four moves.", true);
                    yield return dialogSystem.TypeDialog($"Delete a move to make room for {learnableMove.moveBase.MoveName}?");

                    yield return dialogSystem.SetChoiceBox(() =>
                    {
                        playerSelection = true;
                    }
                    , () =>
                    {
                        playerSelection = false;
                    });

                    if (playerSelection == true)
                    {
                        yield return OpenToLearnNewMove(pokemon, learnableMove.moveBase);

                        if (playerDoesNotWantToLearnMove == false)
                        {
                            yield return dialogSystem.TypeDialog($"{pokemon.currentName} forgot how to use {previousMoveName}", true);
                            yield return dialogSystem.TypeDialog($"{pokemon.currentName} learned {learnableMove.moveBase.MoveName}!", true);
                            playerSelection = false;
                            continue;
                        }
                        else
                        {
                            playerSelection = false;
                        }
                    }

                    if (playerSelection == false)
                    {
                        yield return dialogSystem.TypeDialog($"Stop Learning {learnableMove.moveBase.MoveName}?");
                        yield return dialogSystem.SetChoiceBox(() =>
                        {
                            playerSelection = false;
                        }
                        , () =>
                        {
                            playerSelection = true;
                        });

                        if (playerSelection == false)
                        {
                            yield return dialogSystem.TypeDialog($"{pokemon.currentName} did not learn {learnableMove.moveBase.MoveName}");
                        }
                    }
                }
            }
        }
    }

    public IEnumerator PokemonWantsToLearnNewMoves(Pokemon pokemon, TMHMItem newTMMove)
    {
        yield return dialogSystem.TypeDialog($"{pokemon.currentName} is trying to learn {newTMMove.GetMove.MoveName}.", true);
        yield return dialogSystem.TypeDialog($"But {pokemon.currentName} can't learn more than four moves.", true);
        yield return dialogSystem.TypeDialog($"Delete a move to make room for {newTMMove.GetMove.MoveName}?");

        bool playerSelection = false;

        yield return dialogSystem.SetChoiceBox(() =>
        {
            playerSelection = true;
        }
        , () =>
        {
            playerSelection = false;
        });
        
        if (playerSelection == true)
        {
            yield return OpenToLearnNewMove(pokemon, newTMMove.GetMove);

            playerSelection = playerDoesNotWantToLearnMove;
            if (playerSelection == false)
            {
                yield return dialogSystem.TypeDialog($"{pokemon.currentName} forgot how to use {previousMoveName}", true);
                yield return dialogSystem.TypeDialog($"{pokemon.currentName} learned {newTMMove.GetMove.MoveName}!", true);
                inventorySystem.RemoveItem(newTMMove);
            }
            else
            {
                yield return dialogSystem.TypeDialog($"{pokemon.currentName} did not learn {newTMMove.GetMove.MoveName}", true);
            }
        }
        else
        {
            yield return dialogSystem.TypeDialog($"{pokemon.currentName} did not learn {newTMMove.GetMove.MoveName}", true);
        }
    }

    IEnumerator OpenToLearnNewMove(Pokemon pokemon ,MoveBase newMove)
    {
        Setup(pokemon, newMove);
        gameObject.SetActive(true);
        playerInMenuMakingDecision = false;
        SelectBox();
        yield return new WaitUntil(() => playerInMenuMakingDecision == true);
        gameObject.SetActive(false);
    }

    public void Setup(Pokemon pokemon, MoveBase newMove)
    {
        pokemonSprite.sprite = pokemon.pokemonBase.GetAnimatedSprites()[0];
        pokemonName.text = pokemon.currentName;
        gender.sprite = GlobalArt.ReturnGenderArt(pokemon.gender);
        type1.sprite = GlobalArt.ReturnElementArt(pokemon.pokemonBase.pokemonType1);
        type2.sprite = (pokemon.pokemonBase.pokemonType2 != ElementType.NA) ? 
            GlobalArt.ReturnElementArt(pokemon.pokemonBase.pokemonType2) : GlobalArt.nothing;

        SetMoveReplacementList(pokemon,pokemon.moves,newMove);

        playerDoesNotWantToLearnMove = false;
    }

    public void SetMoveReplacementList(Pokemon pokemon,List<Move> moves,MoveBase newMove)
    {
        for (int i = 0; i < moveButton.Length; i++)
        {
            int k = i;
            if(i == PokemonBase.MAX_NUMBER_OF_MOVES)
            {
                moveButton[i].GetComponent<SummaryAttackButton>().SetMove(newMove);
            }
            else
            {
                moveButton[i].GetComponent<SummaryAttackButton>().SetMove(moves[i]);
            }

            moveButton[i].GetComponent<Button>().onClick.RemoveAllListeners();
            if(i == PokemonBase.MAX_NUMBER_OF_MOVES)
            {
                moveButton[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    //Remove that move, if it is the current move wanting to be learned then stop learning it
                    RefuseToLearnMove();
                });
            }
            else
            {
                moveButton[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    //Remove that move, if it is the current move wanting to be learned then stop learning it
                    previousMove = pokemon.moves[k];
                    pokemon.moves[k] = new Move(newMove);
                    playerInMenuMakingDecision = true;
                });
            }
        }
    }

    void SelectBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(moveButton[0]);
    }

    void RefuseToLearnMove()
    {
        playerDoesNotWantToLearnMove = true;
        playerInMenuMakingDecision = true;
    }

    public string previousMoveName
    {
        get { return previousMove.moveBase.MoveName; }
    }
}
