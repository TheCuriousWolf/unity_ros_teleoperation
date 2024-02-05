using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

public class LidarDrawer : MonoBehaviour
{
    public LidarSpawner lidarSpawner;
    public Mesh mesh;
    public Material material;

    GraphicsBuffer _meshTriangles;
    GraphicsBuffer _meshVertices;
    GraphicsBuffer _ptData;

    public float offset = 4.5f;
    public float scale = 1.0f;
    public int maxPts = 1_000_000;
    public int displayPts = 10;
    private RenderParams renderParams;

    void Start()
    {
        _meshTriangles = new GraphicsBuffer(GraphicsBuffer.Target.Structured, mesh.triangles.Length, 4);
        _meshTriangles.SetData(mesh.triangles);
        _meshVertices = new GraphicsBuffer(GraphicsBuffer.Target.Structured, mesh.vertices.Length, 12);
        _meshVertices.SetData(mesh.vertices);
        _ptData = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxPts, 4*sizeof(float));
        _ptData.SetData(new float[] {
            -1, -1, -1, .2f,
            1, -1, -1, .8f,
            1, 1, -1, .0f,
            -1, 1, -1, .5f,
            -1, -1, 1, .3f,
            1, -1, 1, .7f,
            1, 1, 1, .1f,
            -1, 1, 1 , .6f
        });


        renderParams = new RenderParams(material);
        renderParams.worldBounds = new Bounds(Vector3.zero, Vector3.one * 100);
        renderParams.matProps = new MaterialPropertyBlock();

        renderParams.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.Translate(new Vector3(-offset, 0, 0)));
        renderParams.matProps.SetFloat("_PointSize", scale);
        renderParams.matProps.SetBuffer("_LidarData", _ptData);
        renderParams.matProps.SetInt("_BaseVertexIndex", (int)mesh.GetBaseVertex(0));
        renderParams.matProps.SetBuffer("_Positions", _meshVertices);


        lidarSpawner.PointCloudGenerated += OnPointcloud;
    }

    private void OnValidate() {
        if(renderParams.matProps != null)
        {
            renderParams.matProps.SetFloat("_PointSize", scale);
        }
    }

    private void OnDestroy() {
        _meshTriangles?.Dispose();
        _meshTriangles = null;
        _meshVertices?.Dispose();
        _meshVertices = null;
        _ptData?.Dispose();
        _ptData = null;
    }

    private void Update() {
        Graphics.RenderPrimitivesIndexed(renderParams, MeshTopology.Triangles, _meshTriangles, _meshTriangles.count, (int)mesh.GetIndexStart(0),displayPts);
        
    }

    public void OnPointcloud(PointCloud2Msg pointCloud)
    {
        if(pointCloud.data.Length == 0) return;
        if(pointCloud.data.Length / pointCloud.point_step > maxPts)
        {
            Debug.LogWarning("Point cloud too large, truncating");
        }
        else
        {
            _ptData.SetData(pointCloud.data, 0, 0, maxPts * 4 * sizeof(float));
        }
    }
}
