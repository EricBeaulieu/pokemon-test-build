using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackSelectionEventSelector : MonoBehaviour
{
    [SerializeField] AttackButton [] moveButton;
    [SerializeField] GameObject moveDetails;

    [SerializeField] Text pPText;
    [SerializeField] Text typeText;

    SelectableBoxUI selectableBox;
    BattleSystem battleSystem;

    public void Initialization()
    {
        if(moveButton.Length != 4)
        {
            Debug.LogError("the Amount of moves selectable is not set correctly!");
        }

        for (int i = 0; i < 4; i++)
        {
            moveButton[i].SetPPValues(pPText, typeText);
        }

        selectableBox = new SelectableBoxUI(moveButton[0].gameObject);
        battleSystem = GameManager.instance.GetBattleSystem;
    }

    public void Select()
    {
        selectableBox.SelectBox();
    }

    /// <summary>
    /// Sets up the moves in the location for when the pokemon is going to fight
    /// </summary>
    /// <param name="currentPokemon">The current Battle Unit Pokemon</param>
    /// <param name="moves">All available moves</param>
    public void SetMovesList(BattleUnit currentPokemon, List<Move> moves)
    {
        for (int i = 0; i < moveButton.Length; i++)
        {
            if(i < moves.Count)
            {
                int k = i;
                moveButton[i].gameObject.SetActive(true);
                moveButton[i].SetMove(moves[i]);
                moveButton[i].GetButton.onClick.RemoveAllListeners();
                moveButton[i].GetButton.onClick.AddListener(() => 
                {
                    if(moves[k].pP > 0)
                    {
                        battleSystem.AttackSelected(currentPokemon, moves[k]);
                    }
                    else
                    {
                        battleSystem.AttackHasRunOutOfPP();
                    }

                    selectableBox.Deselect();
                    selectableBox.SetLastSelected(moveButton[k].gameObject);
                });
            }
            else
            {
                moveButton[i].gameObject.SetActive(false);
            }
        }
        selectableBox.SetLastSelected(null);
    }

    public void EnableMoveSelector(bool enabled)
    {
        gameObject.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }
}
