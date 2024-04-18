using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebFunctions : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ReturnHome();

    [DllImport("__Internal")]
    private static extern void ExecuteCallback();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            WebCallback();
        }
    }

    public void ReturnUserHome()
    {
        ReturnHome();
    }

    public void WebCallback()
    {
        ExecuteCallback();
    }
}
