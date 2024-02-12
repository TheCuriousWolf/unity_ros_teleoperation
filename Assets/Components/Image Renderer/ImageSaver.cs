using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ImageSaver))]
public class ImageSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ImageSaver myScript = (ImageSaver)target;
        if(GUILayout.Button("Parse File"))
        {
            myScript.ParseFile();
        }
        if(GUILayout.Button("Move to Frame 0"))
        {
            myScript.MoveToFrame(0);
        }
        if(GUILayout.Button("Move to Frame 1"))
        {
            myScript.MoveToFrame(1);
        }
    }
}
#endif

public class ImageSaver : MonoBehaviour
{
    // Load text file
    public TextAsset textFile;
    private int _width, _height;
    private RenderTexture _renderTexture;

    private List<Vector3> _positions;
    private List<Quaternion> _rotations;

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        ParseFile();
    }

    public void MoveToFrame(int i)
    {
        if(i < 0 || i >= _positions.Count)
        {
            Debug.LogError("Invalid frame index: " + i);
            return;
        }

        transform.position = _positions[i];
        transform.rotation = _rotations[i];
    }

    public void Render()
    {
        if(_camera)
        {
            _camera.Render();
        }
    }

    public void ParseFile()
    {
        // Reads the transforms.json file
        ParseTransforms(textFile.text, out _width, out _height, out _positions, out _rotations);

        Debug.Log("Parsed " + _positions.Count + " frames.");

        if(_camera)
        {
            _renderTexture = new RenderTexture(_width, _height, 24);
            _camera.targetTexture = _renderTexture;
        }        

        // _positions = new Vector3[data.frames.Count];
        // _rotations = new Quaternion[data.frames.Count];

        return;
        // for(int i=0; i<data.frames.Count; i++)
        // {
        //     Frame frame = data.frames[i];
        //     List<List<double>> matrix = frame.transform_matrix;
        //     Debug.Log(matrix);
        //     Matrix4x4 m = new Matrix4x4();
        //     // m.SetRow(0, new Vector4(matrix[0][0], matrix[0][1], matrix[0][2], matrix[0][3]));
        //     // m.SetRow(1, new Vector4(matrix[1][0], matrix[1][1], matrix[1][2], matrix[1][3]));
        //     // m.SetRow(2, new Vector4(matrix[2][0], matrix[2][1], matrix[2][2], matrix[2][3]));
        //     // m.SetRow(3, new Vector4(matrix[3][0], matrix[3][1], matrix[3][2], matrix[3][3]));

        //     Vector3 position = m.GetColumn(3);
        //     Quaternion rotation = Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));

        //     _positions[i] = position;
        //     _rotations[i] = rotation;
        // }
    }

    public static void ParseTransforms(string json, out int width, out int height, out List<Vector3> positions, out List<Quaternion> rotations)
    {
        // Manually parse the JSON string
        string[] lines = json.Split('\n');
        // Initialize variables
        width = 0;
        height = 0;
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();

        // Iterate through each line of the JSON string
        for (int i = 0; i < lines.Length; i++)
        {
            // Skip empty lines
            if (string.IsNullOrEmpty(lines[i]))
            continue;

            
        }
    }
}
