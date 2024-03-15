using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using UnityEngine.InputSystem;
using RosMessageTypes.Sensor;
using RosMessageTypes.JoyManager;

public class JoystickManager : MonoBehaviour
{
    public string topic = "/quest/twist";
    public string joyTopic = "/anyjoy/operator";
    public InputActionReference joystickXY;
    public InputActionReference joystickZR;

    private ROSConnection _ros;

    private TwistStampedMsg _twistMsg;
    private AnyJoyMsg _joyMsg;
    private bool _enabled = false;

    void Start()
    {
        _ros = ROSConnection.GetOrCreateInstance();
        _twistMsg = new TwistStampedMsg();
        _twistMsg.header.frame_id = "base";

        _joyMsg = new AnyJoyMsg();
        _joyMsg.header.frame_id = "base";
        _joyMsg.joy = new JoyMsg();

        _ros.RegisterPublisher<TwistStampedMsg>(topic);
        _ros.RegisterPublisher<AnyJoyMsg>(joyTopic);

    }

    public void SetEnabled(bool enabled)
    {
        _enabled = enabled;
    }

    void Update()
    {
        if(_enabled && (joystickXY.action.IsPressed() || joystickZR.action.IsPressed()))
        {
            Vector2 xy = joystickXY.action.ReadValue<Vector2>();
            Vector2 zr = joystickZR.action.ReadValue<Vector2>();
            _twistMsg.twist.linear.x = xy.y * .85;
            _twistMsg.twist.linear.y = xy.x * .5;
            _twistMsg.twist.angular.z = zr.x;
            Debug.Log("Joystick XY: " + xy);    
            _ros.Send(topic, _twistMsg);

            _joyMsg.joy.axes = new float[] {-xy.x, xy.y, zr.y, zr.x, 0, 0, 0};
            _ros.Send(joyTopic, _joyMsg);
        }
    }

}
