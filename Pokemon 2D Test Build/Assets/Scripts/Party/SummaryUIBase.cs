using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SummaryUIBase : MonoBehaviour
{
    Vector3 originalPosition;
    Vector3 offsetPosition;

    protected float _animationTime = 0.2f;

    protected virtual void Awake()
    {
        originalPosition = transform.localPosition;
        offsetPosition = new Vector3(transform.localPosition.x + offsetXPosDifference(), transform.localPosition.y);
    }

    public abstract float offsetXPosDifference();
    public abstract void SetupData(Pokemon pokemon);

    public void SetPosition(bool on)
    {
        if (on == true)
        {
            transform.localPosition = originalPosition;
        }
        else
        {
            transform.localPosition = offsetPosition;
        }
    }

    public IEnumerator Animate(bool on)
    {
        if (on == true)
        {
            yield return GlobalTools.SmoothTransitionToPosition(transform, originalPosition, _animationTime);
        }
        else
        {
            yield return GlobalTools.SmoothTransitionToPosition(transform, offsetPosition, _animationTime);
        }
    }
}
