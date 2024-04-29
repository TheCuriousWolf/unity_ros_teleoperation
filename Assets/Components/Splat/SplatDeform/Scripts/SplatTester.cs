using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Splat
{
    public Color color = Color.white;
    public Vector3 position;
    public Vector3 scale = Vector3.one;
    public Quaternion rotation = Quaternion.identity;

    public Splat(Vector3 position, Vector3 scale, Quaternion rotation)
    {
        this.position = position;
        this.scale = scale;
        this.rotation = rotation;
    }

    public float Gaussian(Vector3 position)
    {
        // return the value of the gaussian function at the given position
        // the value should be 1 at the center of the splat and 0 at the edge
        // the value should be 0 past the edge of the splat

        float distance = Vector3.Distance(position, this.position);
        float value = Mathf.Exp(-Mathf.PI*distance*distance/(scale.x*scale.x));
        return value;
    }

    public Vector3 Normal(Vector3 position)
    {
        // return the normal of the splat based on its scale and position
        // anything past the scale should have a normal of 0

        Vector3 normal = (position - this.position).normalized;
        normal *= Gaussian(position);
        return normal;
        
    }
}

// [ExecuteInEditMode]
public class SplatTester : MonoBehaviour
{
    public Splat[] splats;
    public bool drawGizmos = true;

    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        // mesh = LidarDrawer._MakePolygon(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            foreach (Splat splat in splats)
            {
                Gizmos.color = splat.color;
                Gizmos.DrawSphere(splat.position, splat.scale.x);
            }
        }
    }
}
