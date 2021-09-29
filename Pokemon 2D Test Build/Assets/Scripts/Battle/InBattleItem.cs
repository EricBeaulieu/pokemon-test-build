using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InBattleItem : MonoBehaviour
{
    [SerializeField] Transform pokeballRoute;
    [SerializeField] ItemBase _itemBase;

    [SerializeField] float speedModifier = 0.5f;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public IEnumerator FollowTheRoute()
    {
        Vector3 p0 = pokeballRoute.GetChild(0).position;
        Vector3 p1 = pokeballRoute.GetChild(1).position;
        Vector3 p2 = pokeballRoute.GetChild(2).position;
        Vector3 p3 = pokeballRoute.GetChild(3).position;
        float tParam = 0f;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            transform.position = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = pokeballRoute.GetChild(3).localPosition;
    }

    public IEnumerator MoveToPosition(Vector3 endPos, float duration)
    {
        Vector3 startingPos = transform.localPosition;
        Vector3 difference = endPos - startingPos;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.localPosition = startingPos + (difference * (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = endPos;
    }

    public IEnumerator PokeballAnimation(bool isOpening,float openingDuration = 0.25f)
    {
        PokeballItem pokeball = (PokeballItem)_itemBase;

        Sprite[] pokeballAnimations = pokeball.CaptureSprites();
        float elapsedTime = 0f;

        int totalAnim = pokeballAnimations.Length - 1;
        float differenceTime = (openingDuration / totalAnim);
        int currentAnimation;

        while (elapsedTime < openingDuration)
        {
            if(isOpening == true)
            {
                currentAnimation = Mathf.FloorToInt(elapsedTime / differenceTime);
            }
            else
            {
                currentAnimation = totalAnim - Mathf.FloorToInt(elapsedTime / differenceTime);
            }
            
            image.sprite = pokeballAnimations[currentAnimation];
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if(isOpening == true)
        {
            image.sprite = pokeballAnimations[pokeballAnimations.Length - 1];
        }
        else
        {
            image.sprite = pokeballAnimations[0];
        }
    }

    public IEnumerator ShakePokeball(bool shakeRight)
    {
        float angle = 25f;

        if(shakeRight == true)
        {
            angle = -angle;
        }
        
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + angle;
        float elapsedTime = 0;
        float duration = 0.33f;

        while (elapsedTime < duration)
        {
            float zRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration) % angle;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float zRotation = Mathf.Lerp(endRotation, startRotation, elapsedTime / duration) % angle;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = Vector3.zero;
    }

    public PokeballItem CurrentPokeball
    {
        get { return (PokeballItem)_itemBase; }
    }

    public IEnumerator FadeItem(bool fadeIn)
    {
        float tempAlpha;
        float animationTime = 1.5f;

        if(fadeIn == true)
        {
            tempAlpha = 0;
            while (tempAlpha < 1)
            {
                tempAlpha += (0.01f * animationTime);
                image.color = image.color.SetAlpha(tempAlpha);
                yield return new WaitForSeconds(0.01f);
            }
            image.color = image.color.SetAlpha(1);
        }
        else
        {
            tempAlpha = 1;
            while (tempAlpha > 0)
            {
                tempAlpha -= (0.01f * animationTime);
                image.color = image.color.SetAlpha(tempAlpha);
                yield return new WaitForSeconds(0.01f);
            }
            image.color = image.color.SetAlpha(0);
        }
    }

    public void SetItemArt()
    {
        switch (_itemBase.GetItemType)
        {
            case itemType.Pokeball:
                PokeballItem pokeball = (PokeballItem)_itemBase;
                Sprite[] pokeballAnimations = pokeball.CaptureSprites();
                image.sprite = pokeballAnimations[0];
                image.color = image.color.SetAlpha(1);
                break;
            default:
                break;
        }
    }

    public string GetItemName
    {
        get { return _itemBase.ItemName; }
    }

    public void SetInBattleItem(ItemBase item)
    {
        _itemBase = item;
    }
}
