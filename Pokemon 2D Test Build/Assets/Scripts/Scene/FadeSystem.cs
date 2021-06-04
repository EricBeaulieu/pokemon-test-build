using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeStyle { FullFade,Clockwise,InnerCircle,HorizontalSplit}

public class FadeSystem : MonoBehaviour
{
    [SerializeField] Image fullFade;
    [SerializeField] Image clockwiseFade;
    [SerializeField] Image innerMaskCircle;
    [SerializeField] Image horizontalSplitTop;
    [SerializeField] Image horizontalSplitBottom;

    const int INNER_MAX_CIRCLE_MAX = 1000;

    public IEnumerator FadeIn(FadeStyle style)
    {
        switch (style)
        {
            case FadeStyle.FullFade:
                yield return FullFadeIn();
                break;
            case FadeStyle.Clockwise:
                yield return ClockwiseIn();
                break;
            case FadeStyle.InnerCircle:
                yield return InnerMaskCircleIn();
                break;
            case FadeStyle.HorizontalSplit:
                yield return HorizontalSplitIn();
                break;
            default:
                break;
        }
    }

    public IEnumerator FadeOut(FadeStyle style)
    {
        TurnAllOtherStylesOff(style);
        switch (style)
        {
            case FadeStyle.FullFade:
                yield return FullFadeOut();
                break;
            case FadeStyle.Clockwise:
                yield return ClockwiseOut();
                break;
            case FadeStyle.InnerCircle:
                yield return InnerMaskCircleOut();
                break;
            case FadeStyle.HorizontalSplit:
                yield return HorizontalSplitOut();
                break;
            default:
                break;
        }
    }

    void TurnAllOtherStylesOff(FadeStyle currentStyle)
    {
        if(currentStyle != FadeStyle.FullFade)
        {
            fullFade.color = fullFade.color.SetAlpha(0);
        }

        if(currentStyle != FadeStyle.Clockwise)
        {
            clockwiseFade.fillAmount = 0;
        }

        if(currentStyle != FadeStyle.InnerCircle)
        {
            innerMaskCircle.GetComponent<RectTransform>().sizeDelta = new Vector2(INNER_MAX_CIRCLE_MAX, INNER_MAX_CIRCLE_MAX);
        }

        if (currentStyle != FadeStyle.HorizontalSplit)
        {
            horizontalSplitTop.fillAmount = 0;
            horizontalSplitBottom.fillAmount = 0;
        }
    }

    #region FullFade

    IEnumerator FullFadeIn()
    {
        float tempAlpha = 0;
        float animationTime = 1.5f;

        while (tempAlpha < 1)
        {
            tempAlpha += (0.01f * animationTime);

            fullFade.color = fullFade.color.SetAlpha(tempAlpha);
            yield return new WaitForSeconds(0.01f);
        }
        fullFade.color = fullFade.color.SetAlpha(1);
    }

    IEnumerator FullFadeOut()
    {
        float tempAlpha = 1;
        float animationTime = 1.5f;

        while (tempAlpha > 0)
        {
            tempAlpha -= (0.01f * animationTime);

            fullFade.color = fullFade.color.SetAlpha(tempAlpha);
            yield return new WaitForSeconds(0.01f);
        }

        fullFade.color = fullFade.color.SetAlpha(0);
    }

    #endregion

    #region Clockwise

    IEnumerator ClockwiseIn()
    {
        float fillAmount = 0;
        float animationTime = 1.5f;

        while (fillAmount < 1)
        {
            fillAmount += (0.01f * animationTime);

            clockwiseFade.fillAmount = fillAmount;
            yield return new WaitForSeconds(0.01f);
        }
        clockwiseFade.fillAmount = 1;
    }

    IEnumerator ClockwiseOut()
    {
        float fillAmount = 1;
        float animationTime = 1.5f;

        while (fillAmount > 0)
        {
            fillAmount -= (0.01f * animationTime);

            clockwiseFade.fillAmount = fillAmount;
            yield return new WaitForSeconds(0.01f);
        }
        clockwiseFade.fillAmount = 0;
    }

    #endregion

    #region InnerMaskCircle

    IEnumerator InnerMaskCircleIn()
    {
        float circleSize = INNER_MAX_CIRCLE_MAX;
        RectTransform rT = innerMaskCircle.GetComponent<RectTransform>();
        float animationTime = 2.5f;

        while (circleSize > 0)
        {
            rT.sizeDelta = new Vector2(circleSize, circleSize);

            circleSize -= (0.01f * animationTime * INNER_MAX_CIRCLE_MAX);
            yield return new WaitForSeconds(0.01f);
        }
        rT.sizeDelta = new Vector2(0, 0);
    }

    IEnumerator InnerMaskCircleOut()
    {
        float circleSize = 0;
        RectTransform rT = innerMaskCircle.GetComponent<RectTransform>();
        float animationTime = 2.5f;

        while (circleSize < INNER_MAX_CIRCLE_MAX)
        {
            rT.sizeDelta = new Vector2(circleSize, circleSize);

            circleSize += (0.01f * animationTime * INNER_MAX_CIRCLE_MAX);
            yield return new WaitForSeconds(0.01f);
        }
        rT.sizeDelta = new Vector2(INNER_MAX_CIRCLE_MAX, INNER_MAX_CIRCLE_MAX);
    }

    #endregion

    #region HorizontalSplit

    IEnumerator HorizontalSplitIn()
    {
        float fillAmount = 0;
        float animationTime = 2f;

        while (fillAmount < 1)
        {
            fillAmount += (0.01f * animationTime);

            horizontalSplitTop.fillAmount = fillAmount/2;
            horizontalSplitBottom.fillAmount = fillAmount/2;
            yield return new WaitForSeconds(0.01f);
        }
        horizontalSplitTop.fillAmount = 0.5f;
        horizontalSplitBottom.fillAmount = 0.5f;
    }

    IEnumerator HorizontalSplitOut()
    {
        float fillAmount = 1;
        float animationTime = 2f;

        while (fillAmount > 0)
        {
            fillAmount -= (0.01f * animationTime);

            horizontalSplitTop.fillAmount = fillAmount / 2;
            horizontalSplitBottom.fillAmount = fillAmount / 2;
            yield return new WaitForSeconds(0.01f);
        }
        horizontalSplitTop.fillAmount = 0;
        horizontalSplitBottom.fillAmount = 0;
    }

    #endregion

}
