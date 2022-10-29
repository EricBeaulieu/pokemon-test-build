using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Portal),true)]
public class PortalEditor : Editor
{
    GameSceneBaseSO previousPortalDestination;
    DestinationIdentifier previousIdentifier;

    void OnEnable()
    {
        previousPortalDestination = ((Portal)target).AlternativeScene;
        previousIdentifier = ((Portal)target).MatchingIdentifier;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying || GlobalTools.isInPrefabStage())
        {
            return;
        }

        if (GUILayout.Button("Snap To Grid"))
        {
            Portal portal = (Portal)target;
            portal.transform.SnapToGrid();
        }

        GameSceneBaseSO currentValue = ((Portal)target).AlternativeScene;
        DestinationIdentifier currentIdentifierValue = ((Portal)target).MatchingIdentifier;

        if (currentValue != previousPortalDestination || currentIdentifierValue != previousIdentifier)
        {
            if (currentValue == null)
            {
                return;
            }

            previousPortalDestination = currentValue;
            previousIdentifier = currentIdentifierValue;
            target.name = $"{PrefabUtility.GetCorrespondingObjectFromSource(target).name} {currentIdentifierValue} to {previousPortalDestination.GetSceneName}";
        }
    }
}
