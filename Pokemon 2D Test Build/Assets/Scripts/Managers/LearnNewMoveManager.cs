using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LearnNewMoveManager : MonoBehaviour
{
    [SerializeField] Image pokemonSprite;
    [SerializeField] Text pokemonName;
    [SerializeField] Image gender;
    [SerializeField] Image type1;
    [SerializeField] Image type2;

    [SerializeField] Text movePower;
    [SerializeField] Text moveAccuracy;
    [SerializeField] Text moveDetails;

    [SerializeField] GameObject[] moveButton;

    public void OpenToLearnNewMove(Pokemon pokemon ,MoveBase newMove)
    {
        Setup(pokemon, newMove);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Setup(Pokemon pokemon, MoveBase newMove)
    {
        pokemonSprite.sprite = pokemon.pokemonBase.GetAnimatedSprites()[0];
        pokemonName.text = pokemon.currentName;
        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(pokemon.gender);
        type1.sprite = StatusConditionArt.instance.ReturnElementArt(pokemon.pokemonBase.pokemonType1);
        if(pokemon.pokemonBase.pokemonType2 != ElementType.NA)
        {
            type2.sprite = StatusConditionArt.instance.ReturnElementArt(pokemon.pokemonBase.pokemonType2);
        }
        else
        {
            type2.sprite = StatusConditionArt.instance.Nothing;
        }

        SetMoveReplacementList(pokemon,pokemon.moves,newMove);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(moveButton[0]);
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
                    Debug.Log("did not learn move");
                    Close();
                });
            }
            else
            {
                moveButton[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    //Remove that move, if it is the current move wanting to be learned then stop learning it
                    pokemon.moves[k] = new Move(newMove);
                    Close();
                });
            }
        }
    }
}
