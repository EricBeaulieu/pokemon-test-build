using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SummaryAttackButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    MoveBase _move;
    public event Action<MoveBase> OnMoveSelected;

    [SerializeField] GameObject selector;
    [SerializeField] Text moveNameText;
    [SerializeField] Text pPText;
    [SerializeField] Image typeSprite;

    public void SetMove(Move move)
    {
        _move = move.moveBase;
        moveNameText.text = move.moveBase.MoveName;
        pPText.text = $"PP{move.pP.ToString()}/{move.moveBase.PowerPoints}";
        typeSprite.sprite = StatusConditionArt.instance.ReturnElementArt(move.moveBase.Type);
    }

    public void SetMove(MoveBase newMove)
    {
        _move = newMove;
        moveNameText.text = newMove.MoveName;
        pPText.text = $"PP{newMove.PowerPoints}/{newMove.PowerPoints}";
        typeSprite.sprite = StatusConditionArt.instance.ReturnElementArt(newMove.Type);
    }

    public void OnSelect(BaseEventData eventData)
    {
        selector.SetActive(true);
        OnMoveSelected?.Invoke(_move);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selector.SetActive(false);
    }
}
