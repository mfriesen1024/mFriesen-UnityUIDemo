using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum colorChannel {r, g, b, a}

public class SliderManager : MonoBehaviour
{
    [SerializeField]colorChannel channel;
    GameObject managerObj;
    // Start is called before the first frame update
    void Start()
    {
        managerObj = GameObject.Find("UI Manager");
        Slider s = GetComponent<Slider>();
        s.onValueChanged.AddListener(SliderUpdate);
    }

    // Update is called once per frame
    void SliderUpdate(float value)
    {
        object[] sendable = {value, channel};
        managerObj.SendMessage("SliderUpdate", sendable);
    }
}
