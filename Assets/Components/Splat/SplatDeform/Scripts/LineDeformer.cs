using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LineDeformer))]
public class LineDeformerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LineDeformer myScript = (LineDeformer)target;
        if(GUILayout.Button("Setup"))
        {
            // myScript.Setup();
        }
    }
}
#endif


public class LineDeformer : MonoBehaviour
{
    public GameObject end;
    public SplatTester splatTester;
    public int segments  = 10;
    public float scale = 1.0f;
    public int interations = 1;


    public void Deform()
    {
                
    }

    void Start()
    {
        // Setup();
    }

    private void OnValidate() {
        // Setup();
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        Vector3[] points = new Vector3[segments];
        Gizmos.color = Color.red;
        Vector3 lastPoint = transform.position;
        for(int i = 0; i < segments; i++)
        {
            points[i] = Vector3.Lerp(transform.position, end.transform.position, (float)i / segments);
            Gizmos.DrawLine(lastPoint, points[i]);
            lastPoint = points[i];
        }

        for(int k = 0; k < interations; k++)
        {
            lastPoint = transform.position;
            for(int i = 0; i < segments; i++)
            {
                Vector3 normal = Vector3.zero;
                foreach(var splat in splatTester.splats)
                {
                    normal += splat.Normal(points[i]);
                }
                Gizmos.color = Color.green;
                Gizmos.DrawLine(points[i], points[i] + normal*scale);

                points[i] += normal*scale;

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(lastPoint, points[i]);
                lastPoint = points[i];
            }
        }



        // for(int i = 0; i < segments; i++)
        // {
        //     // draw a line from the current point to the next point
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawLine(transform.position, end.transform.position);

        // }
    }
}
