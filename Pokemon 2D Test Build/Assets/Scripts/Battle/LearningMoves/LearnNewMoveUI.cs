using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LearnNewMoveUI : MonoBehaviour
{
    [SerializeField] Image pokemonSprite;
    [SerializeField] Text pokemonName;
    [SerializeField] Image gender;
    [SerializeField] Image type1;
    [SerializeField] Image type2;

    [SerializeField] GameObject[] moveButton;

    bool playerInMenuMakingDecision;
    bool playerDoesNotWantToLearnMove = false;
    Move previousMove;

    Action OnFinished;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RefuseToLearnMove();
        }
    }

    public IEnumerator OpenToLearnNewMove(Pokemon pokemon ,MoveBase newMove)
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
        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(pokemon.gender);
        type1.sprite = StatusConditionArt.instance.ReturnElementArt(pokemon.pokemonBase.pokemonType1);
        type2.sprite = (pokemon.pokemonBase.pokemonType2 != ElementType.NA) ? 
            StatusConditionArt.instance.ReturnElementArt(pokemon.pokemonBase.pokemonType2) : StatusConditionArt.instance.Nothing;

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

    public bool PlayerDoesNotWantToLearnMove
    {
        get { return playerDoesNotWantToLearnMove; }
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
