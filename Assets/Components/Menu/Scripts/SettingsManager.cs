using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public PoseManager poseManager;
    public JoystickManager joystickManager;
    public Image axisIcon;
    public Sprite unlockedIcon;
    public Sprite lockedIcon;
    private bool _lockedPose = true;

    void Start()
    {
        poseManager.SetLocked(_lockedPose);
        axisIcon.sprite = _lockedPose ? lockedIcon : unlockedIcon;
        joystickManager.SetEnabled(_lockedPose);
        
    }

    public void Recenter()
    {
        Vector3 position = Camera.main.transform.position;
        position += Camera.main.transform.forward*2;
        position.y = 0;

        poseManager.BaseToLocation(position);
    }

    public void TogglePoseLock()
    {
        _lockedPose = !_lockedPose;
        poseManager.SetLocked(_lockedPose);
        axisIcon.sprite = _lockedPose ? lockedIcon : unlockedIcon;

        joystickManager.SetEnabled(_lockedPose);
    }

}
