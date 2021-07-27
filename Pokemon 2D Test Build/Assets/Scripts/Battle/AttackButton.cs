using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour, ISelectHandler
{
    Move move;
    [SerializeField] Text moveNameText;
    Text pPText;
    Text typeText;
    [SerializeField] Button button;

    Color standardColour = new Color32(50,50,50,255);
    Color orange = new Color32(255, 69, 0,255);

    public void OnSelect(BaseEventData eventData)
    {
        pPText.text = $"PP {move.pP.ToString()}/{move.moveBase.PowerPoints.ToString()}";
        pPText.color = setPpColourText(move);
        typeText.text = $"Type { move.moveBase.Type}";
    }

    public void SetMove(Move currentMove)
    {
        move = currentMove;
        moveNameText.text = currentMove.moveBase.MoveName;
    }

    public void SetPPValues(Text pp,Text type)
    {
        pPText = pp;
        typeText = type;
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
            return orange;
        }
        else
        {
            return standardColour;
        }
    }

    public Button GetButton
    {
        get { return button; }
    }
}
