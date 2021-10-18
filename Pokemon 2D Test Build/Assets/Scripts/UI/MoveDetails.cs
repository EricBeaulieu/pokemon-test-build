using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveDetails : MonoBehaviour
{
    [SerializeField] Image elementTypeSprite;
    [SerializeField] Image moveCategorySprite;
    [SerializeField] Text movePower;
    [SerializeField] Text movePp;
    [SerializeField] Text moveAccuracy;
    [SerializeField] Text moveDetails;

    public void SetData(MoveBase move)
    {
        if(elementTypeSprite != null)
        {
            elementTypeSprite.sprite = StatusConditionArt.instance.ReturnElementArt(move.Type);
        }

        moveCategorySprite.sprite = StatusConditionArt.instance.ReturnMoveCategoryArt(move.MoveType);
        movePower.text = (move.MovePower > 1) ? move.MovePower.ToString() : "--";

        if(movePp != null)
        {
            movePp.text = move.PowerPoints.ToString();
        }

        moveAccuracy.text = (move.AlwaysHits == false) ? move.MoveAccuracy.ToString() : "--";

        if(moveDetails != null)
        {
            moveDetails.text = move.MoveDescription;
        }
    }
}
