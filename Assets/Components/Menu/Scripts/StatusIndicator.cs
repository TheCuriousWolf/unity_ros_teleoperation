using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    public Sprite neutral;
    public Sprite connected;
    public Sprite disconnected;

    private Image _image;
    private RawImage _rawImage;
    void Start()
    {
        _image = GetComponentInChildren<Image>();
        _rawImage = GetComponent<RawImage>();

        _image.sprite = neutral;
        _rawImage.texture = null;
    }
    
    public void OnRosConnection(bool connected)
    {
        _image.sprite = connected ? this.connected : disconnected;
        _rawImage.color = connected ? Color.green : Color.red;
    }
}
