using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAbilityUI : MonoBehaviour
{
    [SerializeField] Image abilityImage;
    Vector3 abilityUIOriginalPosition;
    Vector3 offscreenPos;

    [SerializeField] Text pokemonNameText;
    [SerializeField] Text abilityNameText;

    const float ABILITY_TO_POSITION_TIME = 0.75f;
    const float ABILITY_SHOW_TIME = 3F;

    public void OnStart(bool isPlayers)
    {
        abilityUIOriginalPosition = abilityImage.rectTransform.localPosition;

        if (isPlayers == true)
        {
            offscreenPos = new Vector3(abilityUIOriginalPosition.x - 200f, abilityUIOriginalPosition.y);
        }
        else
        {
            offscreenPos = new Vector3(abilityUIOriginalPosition.x + 200f, abilityUIOriginalPosition.y);
        }

        abilityImage.rectTransform.localPosition = offscreenPos;
    }

    public void OnAbilityActivation(Pokemon pokemon,string abilityName,bool isPlayers)
    {
        pokemonNameText.text = $"{pokemon.currentName}'s";
        abilityNameText.text = $"{abilityName}";

        StartCoroutine(AbilityActivedAnimation(isPlayers));
    }

    IEnumerator AbilityActivedAnimation(bool isPlayers)
    {
        yield return SmoothTransitionToPosition(abilityImage.gameObject, abilityUIOriginalPosition, ABILITY_TO_POSITION_TIME);
        yield return new WaitForSeconds(ABILITY_SHOW_TIME);
        yield return SmoothTransitionToPosition(abilityImage.gameObject, offscreenPos, ABILITY_TO_POSITION_TIME);
    }

    IEnumerator SmoothTransitionToPosition(GameObject gameobject, Vector3 endPos, float duration)
    {
        Transform tempTrans = gameobject.transform;

        Vector3 startingPos = gameobject.transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            tempTrans.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tempTrans.localPosition = endPos;
    }
}
