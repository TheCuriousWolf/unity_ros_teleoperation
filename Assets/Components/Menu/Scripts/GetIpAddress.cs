using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class GetIpAddress : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TMPro.TextMeshProUGUI>();
        _text.text = GetLocalIPv4();
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
    }

    public void ToggleVisible()
    {
        _text.enabled = !_text.enabled;
    }
}
