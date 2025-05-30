using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.AudioCommon;
using System;
using CircularBuffer;

public class AudioReceiver : MonoBehaviour
{
    private ROSConnection ros;
    private AudioSource audioSource;
    private CircularBuffer<float> audioBuffer;
    private string audioDataTopic = "/audio/audio";
    private int sampleRate = 16000;
    private int channelCount = 1;
    private int clipLengthSeconds = 1;
    private AudioClip streamingClip;
    public bool isActive = true;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0.0f;

        // Size enough for at least 1 second of audio
        audioBuffer = new CircularBuffer<float>(sampleRate * 2);

        // Create streaming clip
        streamingClip = AudioClip.Create("StreamingAudio", sampleRate * clipLengthSeconds, channelCount, sampleRate, true, OnAudioRead);
        audioSource.clip = streamingClip;
        audioSource.Play();

        // Connect to ROS
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<AudioDataMsg>(audioDataTopic, ReceiveAudioMessage);
    }

    void ReceiveAudioMessage(AudioDataMsg msg)
    {
        if (!isActive) return;

        byte[] byteData = msg.data;
        float[] floatData = new float[byteData.Length / 2];
        
        // Converting byte data to float data
        for (int i = 0; i < floatData.Length; i++)
        {
            short sample = (short)(byteData[i * 2] | (byteData[i * 2 + 1] << 8));
            floatData[i] = sample / 32768.0f;
        }

        lock (audioBuffer)
        {
            foreach (var sample in floatData)
            {
                audioBuffer.PushBack(sample);
            }
        }
    }

    void OnAudioRead(float[] data)
    {
        lock (audioBuffer)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = audioBuffer.Count > 0 ? audioBuffer.PopFront() : 0.0f;
            }
        }
    }

    void OnDestroy()
    {
        if (ros != null)
        {
            ros.Unsubscribe(audioDataTopic);
        }
    }

    public void toggleSpeaker()
    {
        isActive = !isActive;
    }
}
