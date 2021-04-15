using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryMoves : SummaryUIBase
{
    [SerializeField] GameObject[] moveButton;

    protected override void Awake()
    {
        base.Awake();
        _animationTime /= 2;
    }

    public override float offsetXPosDifference()
    {
        return GetComponent<RectTransform>().sizeDelta.x;
    }

    public override void SetupData(Pokemon pokemon)
    {
        for (int i = 0; i < moveButton.Length; i++)
        {
            if(i < pokemon.moves.Count)
            {
                moveButton[i].GetComponent<SummaryAttackButton>().SetMove(pokemon.moves[i]);
            }
            else
            {
                moveButton[i].GetComponent<SummaryAttackButton>().SetMove();
            }
        }
    }
}
