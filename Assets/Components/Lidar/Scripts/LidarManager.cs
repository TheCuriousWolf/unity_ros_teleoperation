using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LidarManager : MonoBehaviour
{
    public LidarDrawer lidarDrawer;
    public LidarDrawer rgbdDrawer;

    public TMPro.TMP_InputField lidarTopic;
    public TMPro.TMP_InputField rgbdTopic;

    private string _lidarTopic;
    private string _rgbdTopic;

    public GameObject menu;


    void Start()
    {
        _lidarTopic = lidarTopic.text;
        _rgbdTopic = rgbdTopic.text;

        if(PlayerPrefs.HasKey("lidarTopic"))
        {
            _lidarTopic = PlayerPrefs.GetString("lidarTopic");
        }
        if(PlayerPrefs.HasKey("rgbdTopic"))
        {
            _rgbdTopic = PlayerPrefs.GetString("rgbdTopic");
        }

        lidarTopic.text = _lidarTopic;
        rgbdTopic.text = _rgbdTopic;

        lidarDrawer.OnTopicChange(_lidarTopic);
        rgbdDrawer.OnTopicChange(_rgbdTopic);

        menu.SetActive(false);
    }

    public void OnLidarTopic(string topic)
    {
        _lidarTopic = topic;
        lidarDrawer.OnTopicChange(_lidarTopic);
        PlayerPrefs.SetString("lidarTopic", _lidarTopic);
        PlayerPrefs.Save();
    }

    public void OnRGBDTopic(string topic)
    {
        _rgbdTopic = topic;
        rgbdDrawer.OnTopicChange(_rgbdTopic);
        PlayerPrefs.SetString("rgbdTopic", _rgbdTopic);
        PlayerPrefs.Save();
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void Clear()
    {
        // lidarDrawer.SetActive(false);
        // rgbdDrawer.SetActive(false);
    }

    public void Lidar()
    {
        lidarDrawer.ToggleEnabled();
    }

    public void RGBD()
    {
        rgbdDrawer.ToggleEnabled();
    }
}
