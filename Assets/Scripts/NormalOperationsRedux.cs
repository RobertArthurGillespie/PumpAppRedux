using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalOperationsRedux : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip successClip;
    public float successValueLowerBound;
    public float successValueUpperBound;
    public Slider slider;
    public void PlaySuccesAudio()
    {
        if (slider.value > successValueLowerBound&&slider.value<successValueUpperBound)
        {
            audioSource.clip = successClip;
            audioSource.Play();
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
