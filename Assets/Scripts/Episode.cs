using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Episode", menuName = "Episodes", order = 74)]
public class Episode : ScriptableObject
{
    public string EpisodeObjectName;
    public List<string> AdditionalObjectNames = new List<string>();
    public AudioClip EpisodeAudio;
    public string AnimationBool;
    public string CorrectObjectTag;
    public string AnimationIdleStateName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
