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
                _moveButton[i].SetActive(true);
                _moveButton[i].GetComponentInChildren<Text>().text = moves[i].moveBase.moveName;
                _moveButton[i].GetComponentInChildren<AttackButton>().SetMove(moves[i]);
            }
            else
            {
                _moveButton[i].SetActive(false);
            }
        }
    }
}
