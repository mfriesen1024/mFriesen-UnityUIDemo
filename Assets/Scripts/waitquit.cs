using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitquit : MonoBehaviour
{
    float timer = 0;
    [SerializeField] float timeToQuit = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToQuit) { Application.Quit(); }
    }
}
