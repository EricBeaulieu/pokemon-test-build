using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extension
{
    public static void SetAlpha(this Image image, float newAlpha)
    {
        Color color = image.color;
        color.a = newAlpha;
        image.color = color;
    }

    public static void SetAlpha(this Text text, float newAlpha)
    {
        Color color = text.color;
        color.a = newAlpha;
        text.color = color;
    }

    public static void SetDarkness(this Color original, float value)
    {
        Color color = original;
        color.r = value;
        color.g = value;
        color.b = value;
        original = color;
    }

    public static Vector2 GetDirection(this Vector2 original, FacingDirections facingDir)
    {
        switch (facingDir)
        {
            case FacingDirections.Up:
                return Vector2.up;
            case FacingDirections.Down:
                return Vector2.down;
            case FacingDirections.Left:
                return Vector2.left;
            default://Right
                return Vector2.right;
        }
    }

    public static Color Orange()
    {
        return new Color(1, 0.4218157f, 0);
    }

    public static Color orange { get { return new Color(1, 0.4218157f, 0); } }
}
