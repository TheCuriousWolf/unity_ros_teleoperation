using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(StampedPoseManager))]
public class StampedPoseManagerEditor : SensorManagerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        StampedPoseManager stampedPoseManager = (StampedPoseManager)target;
    }
}
#endif

public class StampedPoseManager : SensorManager
{
    
}
