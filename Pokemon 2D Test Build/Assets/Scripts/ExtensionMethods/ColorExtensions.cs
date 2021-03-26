using UnityEngine;

public static class ColorExtensions
{
    public static Color ResetAlpha(this Color original)
    {
        return new Color (original.r,original.g,original.b,1);
    }

    public static Color SetAlpha(this Color original,float newAlpha)
    {
        return new Color(original.r, original.g, original.b, newAlpha);
    }
}
