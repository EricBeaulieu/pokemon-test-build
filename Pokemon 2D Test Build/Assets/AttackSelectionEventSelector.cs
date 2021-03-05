using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackSelectionEventSelector : MonoBehaviour
{
    [SerializeField] GameObject [] _moveButton;

    [SerializeField] Text pPText;
    [SerializeField] Text typeText;

    BattleUnit _battleUnit;
    BattleSystem _battleSystem;//used for reference

    private void Awake()
    {
        if(_moveButton.Length != 4)
        {
            Debug.LogError("the Amount of moves selectable is not set corretly!");
        }

        for (int i = 0; i < 4; i++)
        {
            _moveButton[i].GetComponent<AttackButton>().SetPPValues(pPText, typeText);
        }
    }

    public void SelectFirstBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_moveButton[0]);
    }

    public Button ReturnMoveButton(int moveNumber)
    {
        return _moveButton[moveNumber].GetComponent<Button>();
    }

    public void SetMovesList(List<Move> moves)
    {
        for (int i = 0; i < _moveButton.Length; i++)
        {
            if(i < moves.Count)
            {
                int k = i;
                _moveButton[i].SetActive(true);
                _moveButton[i].GetComponentInChildren<Text>().text = moves[i].moveBase.moveName;
                _moveButton[i].GetComponent<AttackButton>().SetMove(moves[i]);
                _moveButton[i].GetComponent<Button>().onClick.RemoveAllListeners();
                _moveButton[i].GetComponent<Button>().onClick.AddListener(() => 
                {
                    _battleSystem.AttackSelected(_battleUnit.pokemon, moves[k].moveBase);
                    EventSystem.current.SetSelectedGameObject(null);
                });
            }
            else
            {
                _moveButton[i].SetActive(false);
            }
        }
    }

    public void SetReferenceToBattleSystem(BattleSystem battleSystem)
    {
        _battleSystem = battleSystem;
    }

    public void SetReferenceToCurrentPokemon(BattleUnit battleUnit)
    {
        _battleUnit = battleUnit;
    }
}
