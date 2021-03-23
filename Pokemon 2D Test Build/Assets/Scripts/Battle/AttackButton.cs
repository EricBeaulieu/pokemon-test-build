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

    Color _standardColour;
    Color _orange = new Color32(255, 69, 0,255);

    public void OnSelect(BaseEventData eventData)
    {
        _pPText.text = $"PP {_move.pP.ToString()}/{_move.moveBase.PowerPoints.ToString()}";
        _pPText.color = setPpColourText(_move);
        _typeText.text = $"Type { _move.moveBase.Type}";
    }

    public void SetMove(Move move)
    {
        _move = move;
    }

    public void SetPPValues(Text pp,Text type)
    {
        _pPText = pp;
        _typeText = type;
        _standardColour = _pPText.color;
    }

    Color setPpColourText(Move move)
    {
        float movePercentageLeft = (float)move.pP / (float)move.moveBase.PowerPoints;

        if (move.pP == 0)
        {
            return Color.red;
        }
        else if (movePercentageLeft <= 0.4f)
        {
            return _orange;
        }
        else
        {
            return _standardColour;
        }
    }
}
