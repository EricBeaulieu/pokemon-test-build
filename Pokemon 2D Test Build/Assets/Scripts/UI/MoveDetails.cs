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

    public void SetData(MoveBase move)
    {
        elementTypeSprite.sprite = StatusConditionArt.instance.ReturnElementArt(move.Type);
        moveCategorySprite.sprite = StatusConditionArt.instance.ReturnMoveCategoryArt(move.MoveType);
        movePower.text = (move.MovePower > 0) ? move.MovePower.ToString() : "--";
        movePp.text = move.PowerPoints.ToString();
        moveAccuracy.text = (move.AlwaysHits == false) ? move.MoveAccuracy.ToString() : "--";
    }
}
