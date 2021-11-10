using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCPointer : MonoBehaviour
{
    bool pointingDown = true;
    const int offset = 25;
    const int upsideDownOffset = 65;//the size of the image
    const float SPEED_TO_NEW_SELECTION = 0.3f;
    SelectableBoxUI selectableBox;

    public Pokemon currentPokemon { get; private set; }
    [SerializeField] Image pokemonSprite;
    [SerializeField] GameObject holditemGameobject;
    [SerializeField] Transform graphics;
    [SerializeField] RectTransform pointerImage;

    public void Initialization(SelectableBoxUI selectableBoxUI)
    {
        selectableBox = selectableBoxUI;
        currentPokemon = null;
    }

    public void MoveToPosition(Vector2 endPosition,bool aboveUnit = true)
    {
        if(pointingDown != aboveUnit)
        {
            pointerImage.localScale = new Vector3(pointerImage.localScale.x, pointerImage.localScale.y * -1, pointerImage.localScale.z);
            pointingDown = aboveUnit;
        }

        if(pointingDown == true)
        {
            endPosition = new Vector3(endPosition.x, endPosition.y + offset);
        }
        else
        {
            endPosition = new Vector3(endPosition.x, endPosition.y - upsideDownOffset);
        }
        transform.position = endPosition;
    }

    public IEnumerator SelectPokemon(PCPokemon pCPokemon)
    {
        if(currentPokemon == null)
        {
            if (pCPokemon.currentPokemon == null)
            {
                yield break;
            }
            else
            {
                selectableBox.Deselect();
                yield return GlobalTools.SmoothTransitionToPosition(graphics, pCPokemon.gameObject.transform.position, SPEED_TO_NEW_SELECTION);
                currentPokemon = pCPokemon.WithdrawPokemon();
                UpdateData();
                yield return GlobalTools.SmoothTransitionToPosition(graphics, transform.position, SPEED_TO_NEW_SELECTION);
                selectableBox.SelectBox(pCPokemon.gameObject);
            }
        }
        else
        {
            //switch pokemon
            selectableBox.Deselect();
            if (pCPokemon.currentPokemon == null)
            {
                yield return GlobalTools.SmoothTransitionToPosition(graphics, pCPokemon.gameObject.transform.position, SPEED_TO_NEW_SELECTION);
                pCPokemon.DepositPokemon(currentPokemon);
                currentPokemon = null;
                UpdateData();
                yield return GlobalTools.SmoothTransitionToPosition(graphics, transform.position, SPEED_TO_NEW_SELECTION);
            }
            else
            {
                yield return GlobalTools.SmoothTransitionToPosition(graphics, pCPokemon.gameObject.transform.position, SPEED_TO_NEW_SELECTION);
                Pokemon temp = currentPokemon;
                currentPokemon = pCPokemon.WithdrawPokemon();
                pCPokemon.DepositPokemon(temp);
                UpdateData();
                yield return GlobalTools.SmoothTransitionToPosition(graphics, transform.position, SPEED_TO_NEW_SELECTION);
            }
            selectableBox.SelectBox(pCPokemon.gameObject);
        }
    }

    public void UpdateData()
    {
        if (currentPokemon != null)
        {
            pokemonSprite.sprite = currentPokemon.pokemonBase.GetAnimatedSprites()[0];
            holditemGameobject.SetActive(currentPokemon.GetCurrentItem != null);
        }
        else
        {
            pokemonSprite.sprite = StatusConditionArt.instance.Nothing;
            holditemGameobject.SetActive(false);
        }
    }
}
