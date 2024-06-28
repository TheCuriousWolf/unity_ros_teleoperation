using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ModelManager))]
public class ModelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        

        ModelManager myScript = (ModelManager)target;
        if (GUILayout.Button("Change Model"))
        {
        }
    }
}
#endif


public class ModelManager : MonoBehaviour
{
    public static ModelManager instance;
    public Robot currentRobot;
    public Sprite showRobotSprite;
    public Sprite hideRobotSprite;
    public Button toggleModel;
    public bool startVisible = true;

    private bool _enabled;
    private GameObject _currentModel;
    private GameObject _root;
    private Image _toggleImage;

    private void Awake()
    {        
        instance = this;
        _currentModel = Instantiate(currentRobot.modelRoot);
        _root = GameObject.FindWithTag("root");

        toggleModel.onClick.AddListener(ToggleModel);
        _toggleImage = toggleModel.transform.GetChild(0).GetChild(1).GetComponent<Image>();

        _toggleImage.sprite = startVisible ? hideRobotSprite : showRobotSprite;
        _currentModel.SetActive(startVisible);
        _enabled = startVisible;
    }

    public void ChangeModel(int modelIndex)
    {
        currentRobot = Robot.robots[modelIndex];
        Debug.Log("Changed to model of " + currentRobot);

        Destroy(_currentModel);
        _currentModel = Instantiate(currentRobot.modelRoot);

        _currentModel.transform.SetParent(_root.transform);
    }

    public void ToggleModel()
    {
        _enabled = !_enabled;

        _currentModel.SetActive(_enabled);
        _toggleImage.sprite = _enabled ? hideRobotSprite : showRobotSprite;
    }
}
