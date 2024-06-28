using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

#endif

[System.Serializable]
public class Robot
{
    public static Robot[] robots = new Robot[]
    {
            new Robot { RobotName = "ALMA", modelRoot = null, RobotSprite = null },
            new Robot { RobotName = "Dynaarm", modelRoot = null, RobotSprite = null },
            new Robot { RobotName = "Anymal", modelRoot = null, RobotSprite = null },
            new Robot { RobotName = "Panda", modelRoot = null, RobotSprite = null },
    };

    public string RobotName;
    public GameObject modelRoot;
    public Sprite RobotSprite;
    public readonly int id;
    private static int nextId = 0;

    public Robot()
    {
        id = nextId++;
    }

    public static implicit operator int(Robot robot)
    {
        return robot.id;
    }

    public Robot this[int index]
    {
        get => robots[index];
    }

    public string ToString()
    {
        return RobotName;
    }
}