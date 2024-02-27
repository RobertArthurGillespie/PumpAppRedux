using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PluginFunctions : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void NavigateHome();

    public void NavigateToHomepage()
    {
        NavigateHome();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
