using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Nvblox;
using System;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;



#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(NvbloxMesh))]
public class NvbloxMeshEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NvbloxMesh myScript = (NvbloxMesh)target;
        if(GUILayout.Button("Clear"))
        {
            myScript.Clear();
        }
    }
}
#endif


namespace RosMessageTypes.Nvblox
{
    public static class Index3DMsgExtensions
    {
        public static Tuple<int, int, int> ToTuple(this Index3DMsg index)
        {
            return new Tuple<int, int, int>(index.x, index.y, index.z);
        }
    }
}

public class NvbloxMesh : MonoBehaviour
{
    public string topic = "/nvblox_node/mesh";
    public Material material;
    private ROSConnection _ros;
    public bool debug = false;
    public float size = 0.5f;
    private Dictionary<Tuple<int, int, int>, GameObject> blocks;

    public GameObject _parent;

    public bool _enabled = true;


    void Start()
    {

        blocks = new Dictionary<Tuple<int, int, int>, GameObject>();

        _ros = ROSConnection.GetOrCreateInstance();
        _ros.Subscribe<MeshMsg>(topic, UpdateMesh);
    }

    void UpdatePose(string frame)
    {
        _parent = GameObject.Find(frame);
        if(_parent == null) return;

        transform.parent = _parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    void UpdateMesh(MeshMsg tmpMesh)
    {
        // Debug.Log("Updating mesh: " + tmpMesh.mesh_blocks.Length + " blocks.");

        MeshBlockMsg[] meshBlocks = tmpMesh.blocks;
        float block_size = tmpMesh.block_size;
        bool clear = tmpMesh.clear;

        if(clear)
        {
            Debug.Log("Clearing mesh");
            Clear();
        }

        if(_parent == null || _parent.name != tmpMesh.header.frame_id)
        {
            UpdatePose(tmpMesh.header.frame_id);
        }


        string blocksStr = "";

        Mesh mesh;

        for(int j=0; j<meshBlocks.Length; j++)
        {
            MeshBlockMsg block = meshBlocks[j];
            Color color;
            
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Color> colors = new List<Color>();
            int vertexCount = 0;

            Tuple<int, int, int> index = tmpMesh.block_indices[j].ToTuple();

            if(!blocks.ContainsKey(index))
            {
                GameObject blockObj = new GameObject("Block " + index.Item1 + " " + index.Item2 + " " + index.Item3);
                blockObj.transform.parent = transform;
                blockObj.transform.localPosition = Vector3.zero;
                blockObj.transform.localRotation = Quaternion.identity;
                blockObj.transform.localScale = Vector3.one;
                blocks.Add(index, blockObj);

                mesh = new Mesh();
                blockObj.AddComponent<MeshFilter>().mesh = mesh;
                blockObj.AddComponent<MeshRenderer>().material = material;
                MeshCollider collider = blockObj.AddComponent<MeshCollider>();
                collider.sharedMesh = mesh;
                collider.convex = true;

            } else
            {
                mesh = blocks[index].GetComponent<MeshFilter>().mesh;
            }

            mesh.Clear();



            for(int i=0; i<block.vertices.Length; i++)
            {
                float x = block.vertices[i].x;
                float y = block.vertices[i].y;
                float z = block.vertices[i].z;

                // switch between ros and unity coordinates
                Vector3 pos = new Vector3<FLU>(x, y, z).toUnity;
                vertices.Add(pos);

                if(block.normals.Length > 0)
                {
                    x = block.normals[i].x;
                    y = block.normals[i].y;
                    z = block.normals[i].z;
                    Vector3 normal = new Vector3<FLU>(x, y, z).toUnity;
                    normals.Add(normal);
                }

                if(block.colors.Length > 0)
                {
                    color = new Color(block.colors[i].r, block.colors[i].g, block.colors[i].b);
                } else
                {
                    color = new Color(.5f, .5f, .5f);
                }
                colors.Add(color);
            }



            if(block.triangles.Length > 0)
            {
                triangles.AddRange(block.triangles);
            }

                
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            if(block.normals.Length > 0)
            {
                mesh.normals = normals.ToArray();
            }
            mesh.colors = colors.ToArray();
            mesh.RecalculateBounds();
        }

    }

    public void ToggleEnabled()
    {
        _enabled = !_enabled;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(_enabled);
        }
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        blocks.Clear();
    }

}
