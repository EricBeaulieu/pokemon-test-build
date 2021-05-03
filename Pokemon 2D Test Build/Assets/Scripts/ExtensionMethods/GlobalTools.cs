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
}
