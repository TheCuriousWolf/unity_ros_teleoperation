using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Color[] colors = new Color[mesh.vertices.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = i%2==0?Color.red:Color.green;
        }        
        mesh.colors = colors;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
