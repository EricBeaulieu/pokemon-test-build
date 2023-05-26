using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraHelper))]
public class CameraHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CameraHelper currentCam = (CameraHelper)target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Set Camera to ScreenShot"))
        {
            currentCam.SetToScreenShotView();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Set Camera to Standard"))
        {
            currentCam.SetToStandardView();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Center Camera"))
        {
            currentCam.CenterCamera();
        }

        GUILayout.EndHorizontal();
    }
}
