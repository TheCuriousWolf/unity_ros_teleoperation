using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(GridMapManager))]
public class GridMapManagerEditor : SensorManagerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridMapManager gridMapManager = (GridMapManager)target;
    }
}
#endif

public class GridMapManager : SensorManager
{
}
