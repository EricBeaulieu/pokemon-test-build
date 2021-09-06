using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GlobalTools
{
    public static IEnumerator SmoothTransitionToPosition(Transform curTransform, Vector3 endPos, float duration)
    {
        Vector3 startingPos = curTransform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            curTransform.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        curTransform.localPosition = endPos;
    }

    public static string SplitCamelCase(string inputCamelCaseString)
    {
        string sTemp = Regex.Replace(inputCamelCaseString, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
        return Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim();
    }

    public static string ReplaceUserWithPokemonName(string original,Pokemon user)
    {
        if(original.Contains("<user>") == true)
        {
            return original.Replace("<user>", user.currentName);
        }
        return original;
    }

    public static Vector3 SnapToGrid(Vector3 currentPos)
    {
        currentPos.x = Mathf.FloorToInt(currentPos.x) + Entity.TILE_CENTER_OFFSET;
        currentPos.y = Mathf.FloorToInt(currentPos.y) + Entity.TILE_CENTER_OFFSET;
        currentPos.z = 0;

        return currentPos;
    }
}
