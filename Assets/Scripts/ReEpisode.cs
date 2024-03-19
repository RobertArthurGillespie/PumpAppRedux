using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ReEpisode", menuName = "ReEpisodes", order = 76)]
public class ReEpisode : ScriptableObject
{
    public string EpisodeObjectName;
    public List<string> AdditionalObjectNames = new List<string>();
    public List<string> TransparentObjectNames = new List<string>();
    public AudioClip EpisodeAudio;
    public string AnimationBool;
    public string CorrectObjectTag;
    public string AnimationName;
    public string IdleAnimationName;
    public bool NoAnimation = false;
    public bool OtherAnimator = false;
    public string OtherAnimatorName;
    public List<string> ExtensionMethods = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
