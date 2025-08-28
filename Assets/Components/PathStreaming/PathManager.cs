using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

class PathManagerEditor : SensorManagerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif


public class PathManager : SensorManager
{
}
