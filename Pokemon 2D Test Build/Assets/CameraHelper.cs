using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    [SerializeField] Camera cam;
    /// <summary>
    /// true is setting it for standard view which is 6, natural gameplay.
    /// false is for screenshot view which is 16.28
    /// </summary>
    bool currentView = false;

    float GetSize
    {
        get
        {
            if (currentView == true)
            {
                return 6;
            }
            else return 16.28f;
        }
    }

    public void SetToScreenShotView()
    {
        currentView = false;
        cam.orthographicSize = GetSize;
    }

    public void SetToStandardView()
    {
        currentView = true;
        cam.orthographicSize = GetSize;
    }

    public void CenterCamera()
    {
        Transform curTransform = GetComponent<Transform>();

        float xSize = curTransform.localPosition.x * 2;
        float ySize = curTransform.localPosition.y * 2;

        xSize = Mathf.Round(xSize);
        ySize = Mathf.Round(ySize);

        curTransform.localPosition = new Vector3(xSize / 2, ySize / 2, -10);
    }
}
