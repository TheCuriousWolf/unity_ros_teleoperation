using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine.InputSystem;
using RosMessageTypes.Geometry;
using RosMessageTypes.Tf2;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;


public class HeadsetPublisher : MonoBehaviour
{
    public string unityFrame = "vr_origin";
    public string headsetFrame = "headset";
    public string handFrameLeft = "hand_left";

    public InputActionReference headsetPose;
    public InputActionReference headsetRotation;
    public InputActionReference handPoseLeft;
    public InputActionReference handRotationLeft;
    public InputActionReference handPoseRight;
    public InputActionReference handRotationRight;

    private Transform root;
    private string handFrameRight = "hand_right";
    private ROSConnection ros;
    private TFMessageMsg tfMsg;
    private PoseStampedMsg poseMsg;
    private HeaderMsg headHeader;
    private string rootFrame;

    void Awake()
    {
        ros = ROSConnection.GetOrCreateInstance();

        handFrameRight = handFrameLeft.Replace("left", "right");

        root = GameObject.FindWithTag("root").transform;
        rootFrame = root.GetComponent<TFAttachment>().FrameID;

        ros.RegisterPublisher<PoseStampedMsg>(unityFrame + "/" + headsetFrame);
        ros.RegisterPublisher<PoseStampedMsg>(unityFrame + "/" + handFrameLeft);
        ros.RegisterPublisher<PoseStampedMsg>(unityFrame + "/" + handFrameRight);

        ros.RegisterPublisher<TFMessageMsg>("/tf");

        poseMsg = new PoseStampedMsg();
        headHeader = new HeaderMsg();
        headHeader.frame_id = rootFrame;

        tfMsg = new TFMessageMsg(); 

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        tfMsg.transforms = new TransformStampedMsg[2];
        tfMsg.transforms[0] = new TransformStampedMsg();
        HeaderMsg rootHeader = new HeaderMsg();
        rootHeader.frame_id = rootFrame;
        tfMsg.transforms[0].header = rootHeader;
        tfMsg.transforms[0].child_frame_id = unityFrame;
        tfMsg.transforms[0].transform = new TransformMsg();
        tfMsg.transforms[0].transform.translation = new Vector3Msg();
        tfMsg.transforms[0].transform.rotation = new QuaternionMsg();

        // Vector3 rootPosition = 
        tfMsg.transforms[0].transform.translation = root.InverseTransformPoint(Vector3.zero).To<FLU>();
        // tfMsg.transforms[0].transform.translation.y = rootPosition.y;
        // tfMsg.transforms[0].transform.translation.z = rootPosition.z;

        // Quaternion rootRotation = ;
        tfMsg.transforms[0].transform.rotation = root.rotation.To<FLU>();
        // tfMsg.transforms[0].transform.rotation.y = rootRotation.y;
        // tfMsg.transforms[0].transform.rotation.z = rootRotation.z;
        // tfMsg.transforms[0].transform.rotation.w = rootRotation.w;

        tfMsg.transforms[1] = new TransformStampedMsg();
        HeaderMsg headsetHeader = new HeaderMsg();
        headsetHeader.frame_id = unityFrame;
        tfMsg.transforms[1].header = headsetHeader;
        tfMsg.transforms[1].child_frame_id = headsetFrame;
        tfMsg.transforms[1].transform = new TransformMsg();
        tfMsg.transforms[1].transform.translation = new Vector3Msg();
        tfMsg.transforms[1].transform.rotation = new QuaternionMsg();






        poseMsg.header = headHeader;
        PointMsg point = headsetPose.action.ReadValue<Vector3>().To<FLU>();

        tfMsg.transforms[1].transform.translation = headsetPose.action.ReadValue<Vector3>().To<FLU>();
        
        QuaternionMsg quaternion = headsetRotation.action.ReadValue<Quaternion>().To<FLU>();

        tfMsg.transforms[1].transform.rotation = headsetRotation.action.ReadValue<Quaternion>().To<FLU>();

        poseMsg.pose.position = point;
        poseMsg.pose.orientation = quaternion;
        ros.Publish(unityFrame + "/" + headsetFrame, poseMsg);

        ros.Publish("/tf", tfMsg);
    }
}
