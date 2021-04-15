using UnityEngine;

public static class ColorExtensions
{
    public static Color SetAlpha(this Color original,float newAlpha)
    {
        return new Color(original.r, original.g, original.b, newAlpha);
    }
}
