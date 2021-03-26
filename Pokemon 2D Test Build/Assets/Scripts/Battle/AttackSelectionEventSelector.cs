using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackSelectionEventSelector : MonoBehaviour
{
    [SerializeField] GameObject [] moveButton;

    [SerializeField] Text pPText;
    [SerializeField] Text typeText;

    GameObject _lastSelected;

    private void Awake()
    {
        if(moveButton.Length != 4)
        {
            Debug.LogError("the Amount of moves selectable is not set corretly!");
        }

        for (int i = 0; i < 4; i++)
        {
            moveButton[i].GetComponent<AttackButton>().SetPPValues(pPText, typeText);
        }
    }

    /// <summary>
    /// When the Fight Button is clicked it shall select the last attack used
    /// It shall select the first move if there have been no attacks used
    /// </summary>
    public void SelectBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (_lastSelected == null)
        {
            EventSystem.current.SetSelectedGameObject(moveButton[0]);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected);
        }
    }

    //public Button ReturnMoveButton(int moveNumber)
    //{
    //    return moveButton[moveNumber].GetComponent<Button>();
    //}

    /// <summary>
    /// Sets up the moves in the location for when the pokemon is going to fight
    /// </summary>
    /// <param name="currentPokemon">The current Battle Unit Pokemon</param>
    /// <param name="moves">All available moves</param>
    /// <param name="currentBattleSystem">A reference to the battle system that gets passed down</param>
    public void SetMovesList(BattleUnit currentPokemon, List<Move> moves,BattleSystem currentBattleSystem)
    {
        for (int i = 0; i < moveButton.Length; i++)
        {
            if(i < moves.Count)
            {
                int k = i;
                moveButton[i].SetActive(true);
                moveButton[i].GetComponentInChildren<Text>().text = moves[i].moveBase.MoveName;
                moveButton[i].GetComponent<AttackButton>().SetMove(moves[i]);
                moveButton[i].GetComponent<Button>().onClick.RemoveAllListeners();
                moveButton[i].GetComponent<Button>().onClick.AddListener(() => 
                {
                    if(moves[k].pP > 0)
                    {
                        currentBattleSystem.AttackSelected(currentPokemon, moves[k].moveBase);
                        moves[k].pP--;
                    }
                    else
                    {
                        currentBattleSystem.AttackHasRunOutOfPP();
                    }

                    EventSystem.current.SetSelectedGameObject(null);
                    _lastSelected = moveButton[k];
                });
            }
            else
            {
                moveButton[i].SetActive(false);
            }
        }
        _lastSelected = null;
    }
}
