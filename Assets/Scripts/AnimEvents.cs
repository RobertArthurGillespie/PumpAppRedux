using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public List<GameObject> PipeObjects;
    public void RenderPipes()
    {
        foreach(GameObject g in PipeObjects)
        {
            g.GetComponent<MeshRenderer>().enabled = true;
        }
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
