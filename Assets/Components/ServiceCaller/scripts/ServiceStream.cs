using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ServiceStream))]
public class ServiceStreamEditor : SensorStreamEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ServiceStream myScript = (ServiceStream)target;
        if (GUILayout.Button("Subscribe to Service"))
        {
            myScript.SubscribeToService();
        }
        if (GUILayout.Button("Trigger Service"))
        {
            myScript.TriggerService();
        }
    }
}
#endif

public class ServiceStream : SensorStream
{
    public string topic = "/service_name";
    public TextMeshProUGUI topicText;
    public TMPro.TMP_Text topicInputField;
    private ROSConnection _ros;


    // Start is called before the first frame update
    void Start()
    {
        topicText.text = topic;
        topicInputField.text = topic;

        _ros = ROSConnection.GetOrCreateInstance();
    }

    public void OnTopicChanged(string newTopic)
    {
        topic = newTopic;
        topicText.text = topic;
        topicInputField.text = topic;
        Debug.Log($"Topic changed to: {topic}");
    }

    public void SubscribeToService()
    {
        Debug.Log($"Subscribing to service: {topic}");
        _ros.RegisterRosService<TriggerRequest, TriggerResponse>(topic);
    }

    public void TriggerService()
    {
        Debug.Log($"Triggering service: {topic}");
        _ros.SendServiceMessage<TriggerResponse>(topic, new TriggerRequest(), ServiceCallback);
    }

    private void ServiceCallback(TriggerResponse response)
    {
        Debug.Log($"Service response received: {response}");
    }

    public override string Serialize()
    {
        // Add your serialization logic here
        Debug.Log("Serializing ServiceStream");
        return "{}"; // Example return value
    }

    // Implementation of abstract method Deserialize
    public override void Deserialize(string data)
    {
        // Add your deserialization logic here
        Debug.Log($"Deserializing ServiceStream with data: {data}");
    }
    
    public override void ToggleTrack(int trackId)
    {
        // Add your logic here
        Debug.Log($"Toggling track with ID: {trackId}");
    }
}
