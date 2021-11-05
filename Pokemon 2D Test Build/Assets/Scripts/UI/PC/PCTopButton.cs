using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCTopButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] Button button;

    public void OnSelect(BaseEventData eventData)
    {
        PCSystem.pointer.MoveToPosition(this.transform.position, false);
        PCSystem.pCPokemonDataDisplay.SetupData(null);
    }

    public Button GetButton()
    {
        return button;
    }
}
