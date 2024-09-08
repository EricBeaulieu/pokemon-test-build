using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GlobalTools
{
    public static IEnumerator SmoothTransitionToPosition(Transform curTransform, Vector3 endPos, float duration)
    {
        Vector3 startingPos = curTransform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            curTransform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        curTransform.position = endPos;
    }

    public static IEnumerator SmoothTransitionToPositionUsingLocalPosition(Transform curTransform, Vector3 endPos, float duration)
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

    public static void SnapToGrid(this Transform currentTran)
    {
        Vector3 currentPos = currentTran.position;
        currentTran.position = new Vector3(Mathf.FloorToInt(currentPos.x) + Entity.TILE_CENTER_OFFSET, Mathf.FloorToInt(currentPos.y) + Entity.TILE_CENTER_OFFSET, 0);
    }

    public static FacingDirections CurrentDirectionFacing(Animator entityAnim)
    {
        Vector2 facingDirection = new Vector2(entityAnim.GetFloat("moveX"), entityAnim.GetFloat("moveY"));

        switch (facingDirection)
        {
            case Vector2 v when v.Equals(Vector2.up):
                return FacingDirections.Up;
            case Vector2 v when v.Equals(Vector2.down):
                return FacingDirections.Down;
            case Vector2 v when v.Equals(Vector2.left):
                return FacingDirections.Left;
            default:
                return FacingDirections.Right;
        }
    }

    public static FacingDirections CurrentDirectionFacing(Vector2 original, Vector2 target)
    {
        Vector2 curDir = new Vector2(Mathf.Round(target.x - original.x), Mathf.Round(target.y - original.y));

        if(Mathf.Abs(curDir.y) > Mathf.Abs(curDir.x))
        {
            if(curDir.y > 0)
            {
                return FacingDirections.Up;
            }
            else
            {
                return FacingDirections.Down;
            }
        }
        else
        {
            if (curDir.x > 0)
            {
                return FacingDirections.Right;
            }
            else
            {
                return FacingDirections.Left;
            }
        }
    }

    public static Vector2 CurrentDirectionFacing(FacingDirections facing)
    {
        Vector2 dir = new Vector2();
        switch (facing)
        {
            case FacingDirections.Up:
                dir = Vector2.up;
                break;
            case FacingDirections.Down:
                dir = Vector2.down;
                break;
            case FacingDirections.Left:
                dir = Vector2.left;
                break;
            default://FacingDirections.Right
                dir = Vector2.right;
                break;
        }
        return dir;
    }

    public static string FacingDirectionEditorHelper(FacingDirections direction)
    {
        switch (direction)
        {
            case FacingDirections.Up:
                return "Up_Idle";
            case FacingDirections.Down:
                return "Down_Idle";
            case FacingDirections.Left:
                return "Left_Idle";
            default:
                return "Right_Idle";
        }
    }

    public static FacingDirections GetDirectionFacingOnStart(Entity entity)
    {
        string facing;
        if (entity.GetComponentInChildren<SpriteRenderer>().sprite == null)
        {
            facing = "Down_Idle";
        }
        else
        {
            facing = entity.GetComponentInChildren<SpriteRenderer>().sprite.name;
        }

        switch (facing)
        {
            case "Up_Idle":
                return FacingDirections.Up;
            case "Left_Idle":
                return FacingDirections.Left;
            case "Right_Idle":
                return FacingDirections.Right;
            default:
                return FacingDirections.Down;//Make down the default
        }
    }

    public static string ReplacePokemonWithPokemonName(string original, PokemonBase pokemon)
    {
        if (original.Contains("<pokemon>") == true)
        {
            return original.Replace("<pokemon>", pokemon.GetPokedexName());
        }
        return original;
    }

    public static string ReplacePlayerWithTrainerName(string original, PlayerController player)
    {
        if (original.Contains("<player>") == true)
        {
            return original.Replace("<player>", player.TrainerName);
        }
        return original;
    }

    public static bool IsOnTheSameXOrYGrid(this Vector2 original, Vector2 target)
    {
        if (Mathf.FloorToInt(original.x) == Mathf.FloorToInt(target.x) || Mathf.FloorToInt(original.y) == Mathf.FloorToInt(target.y))
        {
            return true;
        }
        return false;
    }

    public static float CalculateGenderRatio(int genderRatio)
    {
        switch (genderRatio)
        {
            case 1:
                return 100;
            case 2:
                return 88.14f;
            case 3:
                return 75.49f;
            case 4:
                return 50.2f;
            case 6:
                return 24.9f;
            case 7:
                return 11.2f;
            default:
                return 0;
        }
    }

    public static string UpperFirstChar(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return null;
        }

        return char.ToUpper(s[0]) + s.Substring(1);
    }

    public static bool isInPrefabStage()
    {
#if UNITY_2018_3_OR_NEWER
        var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        return stage != null;
#else
    return false;
#endif
    }
}
