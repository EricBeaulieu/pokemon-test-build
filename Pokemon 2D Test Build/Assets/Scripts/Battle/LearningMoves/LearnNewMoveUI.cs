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

    //[SerializeField] MoveDetails moveDetails;
    [SerializeField] Text movePower;
    [SerializeField] Text moveAccuracy;
    [SerializeField] Text moveDetails;

    [SerializeField] GameObject[] moveButton;

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

    public void OpenToLearnNewMove(Pokemon pokemon ,MoveBase newMove,Action finished)
    {
        Setup(pokemon, newMove);
        gameObject.SetActive(true);
        OnFinished = finished;
    }

    public void Close()
    {
        if(BattleSystem.inBattle == true)
        {

        }
        else
        {
            //Return to party Screen in certain state
        }
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

    void SetupMoveDetails(MoveBase currentMove)
    {
        movePower.text = currentMove.MovePower.ToString();
        moveAccuracy.text = currentMove.MoveAccuracy.ToString();
        moveDetails.text = currentMove.MoveDescription;
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

            moveButton[i].GetComponent<SummaryAttackButton>().OnMoveSelected += SetupMoveDetails;
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
                    Close();
                    OnFinished?.Invoke();
                });
            }
        }
    }

    public void SelectBox()
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
        Close();
        OnFinished?.Invoke();
    }

    public string previousMoveName
    {
        get { return previousMove.moveBase.MoveName; }
    }
}
