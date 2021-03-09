using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour, ISelectHandler
{
    Move _move;
    Text _pPText;
    Text _typeText;

    public void OnSelect(BaseEventData eventData)
    {
        _pPText.text = $"PP {_move.pP.ToString()}/{_move.moveBase.powerPoints.ToString()}";
        _typeText.text = $"Type { _move.moveBase.type}";
    }

    public void SetMove(Move move)
    {
        _move = move;
    }

    public void SetPPValues(Text pp,Text type)
    {
        _pPText = pp;
        _typeText = type;
    }
}
