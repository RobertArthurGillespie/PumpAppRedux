using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TitratorReduxManager : MonoBehaviour
{
    public GameObject pumpAnimator;
    public GameObject pump;
    public List<ReEpisode> PumpReEpisodes = new List<ReEpisode>();
    public AudioSource audioSource;
    public int ReEpisodeIndex = 0;
    public AudioClip introClip;
    public Material RegularMaterial;
    public Material TransMaterial;
    public Material HighlightMaterial;
    public bool introAudioFinished = false;
    public bool hasSkinnedMeshRenderer = false;
    GameObject ReEpisodeObject;

    public void PlayNextEpisode()
    {
        StartCoroutine(PlayReEpisodeCoroutine());
    }

    public IEnumerator PlayReEpisodeCoroutine()
    {
        /*while (true)
        {
            if (pump.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !pump.GetComponent<Animator>().IsInTransition(0))
            {

                if (!audioSource.isPlaying)
                {
                    Debug.Log("transition animation and audio is finished");
                    


                    break;
                }

            }
            yield return null;
        }*/
        Material[] MatArray = new Material[1];
        

        if (!introAudioFinished)
        {
            audioSource.clip = introClip;
            audioSource.Play();
            //GameObject.Find("SimScriptText").GetComponent<TextMeshProUGUI>().text = "Let's start by looking at the basic parts of a pump; click each item to put together the pump.";
            while (true)
            {
                if (!audioSource.isPlaying)
                {
                    introAudioFinished = true;
                    break;
                }
                yield return null;
            }
        }
        ReEpisode currentReEpisode = PumpReEpisodes[ReEpisodeIndex];

        ReEpisodeObject = GameObject.Find(currentReEpisode.EpisodeObjectName);
        ReEpisodeObject.GetComponent<Collider>().enabled = true;
        Debug.Log("ReEpisode object is: " + ReEpisodeObject.name);
        if (currentReEpisode.TransparentObjectNames.Count > 0)
        {
            foreach (string name in currentReEpisode.TransparentObjectNames)
            {
                GameObject transpObject = GameObject.Find(name);
                

                if (transpObject.GetComponent<MeshRenderer>().materials.Count() > 0)
                {
                    Material[] TransMatArray = new Material[2];
                    TransMatArray[0] = TransMaterial;
                    TransMatArray[1] = TransMaterial;
                    transpObject.GetComponent<MeshRenderer>().materials = TransMatArray;
                }
                else
                {
                    transpObject.GetComponent<MeshRenderer>().material = TransMaterial;
                }
            }

        }

        /*ReEpisodeObject.AddComponent<Outline>();
        Outline outlineToEdit = ReEpisodeObject.GetComponent<Outline>();
        outlineToEdit.OutlineColor = new Color(1.60200012f, 1.57944012f, 0, 1);
        outlineToEdit.OutlineWidth = 8f;*/
        
        if (currentReEpisode.HasSkinnedMeshRenderer)
        {
            MatArray = ReEpisodeObject.GetComponent<SkinnedMeshRenderer>().materials;
            RegularMaterial = MatArray[0];
            MatArray[0] = HighlightMaterial;
            ReEpisodeObject.GetComponent<SkinnedMeshRenderer>().materials = MatArray;
        }
        else
        {
            MatArray = ReEpisodeObject.GetComponent<MeshRenderer>().materials;
            RegularMaterial = MatArray[0];
            MatArray[0] = HighlightMaterial;
            ReEpisodeObject.GetComponent<MeshRenderer>().materials = MatArray;
        }
        
        if (currentReEpisode.AdditionalObjectNames.Count > 0)
        {
            foreach (string name in currentReEpisode.AdditionalObjectNames)
            {
                if (currentReEpisode.HasSkinnedMeshRenderer)
                {
                    GameObject obj = GameObject.Find(name);
                    Material[] GlowMatArray = obj.GetComponent<SkinnedMeshRenderer>().materials;
                    GlowMatArray[1] = HighlightMaterial;
                    obj.GetComponent<SkinnedMeshRenderer>().materials = GlowMatArray;
                }
                else
                {
                    GameObject obj = GameObject.Find(name);
                    Material[] GlowMatArray = obj.GetComponent<MeshRenderer>().materials;
                    GlowMatArray[1] = HighlightMaterial;
                    obj.GetComponent<MeshRenderer>().materials = GlowMatArray;
                }
                
            }
        }
        Debug.Log("outline added");
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {



                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))

                {


                    Debug.Log(hit.transform.name);


                    Debug.Log("hit, tag is: " + hit.transform.tag);
                    if (hit.transform.tag == currentReEpisode.CorrectObjectTag)
                    {
                        GameObject.Find("SimScriptText").GetComponent<TextMeshProUGUI>().text = currentReEpisode.EpisodeText;
                        
                        MatArray[0] = RegularMaterial;
                        if (currentReEpisode.HasSkinnedMeshRenderer)
                        {
                            ReEpisodeObject.GetComponent<SkinnedMeshRenderer>().materials = MatArray;
                        }
                        else
                        {
                            ReEpisodeObject.GetComponent<MeshRenderer>().materials = MatArray;
                        }
                        
                        if (currentReEpisode.AdditionalObjectNames.Count > 0)
                        {
                            foreach (string name in currentReEpisode.AdditionalObjectNames)
                            {
                                GameObject obj = GameObject.Find(name);
                                Material[] RegMatArray = obj.GetComponent<MeshRenderer>().materials;
                                RegMatArray[1] = RegularMaterial;
                                obj.GetComponent<MeshRenderer>().materials = RegMatArray;
                            }
                        }
                        //ReEpisodeObject.GetComponent<Outline>().enabled = false;


                        /*if (currentReEpisode.AdditionalObjectNames.Count > 0)
                        {
                            foreach (string name in currentReEpisode.AdditionalObjectNames)
                            {
                                Debug.Log("name of additional object is: " + name);
                                GameObject subObject = GameObject.Find(name);
                                subObject.AddComponent<Outline>();
                                Outline subOutlineToEdit = ReEpisodeObject.GetComponent<Outline>();
                                subOutlineToEdit.OutlineColor = new Color(1.60200012f, 1.57944012f, 0, 1);
                                subOutlineToEdit.OutlineWidth = 8f;
                                subObject.GetComponent<Outline>().enabled = false;
                            }
                        }
                        if(currentReEpisode.TransparentObjectNames.Count > 0)
                        {
                            foreach(string name in currentReEpisode.TransparentObjectNames)
                            {
                                GameObject transpObject = GameObject.Find(name);
                                transpObject.GetComponent<MeshRenderer>().material = RegularMaterial;
                            }
                        }*/
                        if (!currentReEpisode.NoAnimation)
                        {
                            pumpAnimator.GetComponent<Animator>().SetBool(currentReEpisode.AnimationBool, true);
                        }

                        audioSource.clip = currentReEpisode.EpisodeAudio;
                        audioSource.Play();
                        Debug.Log("playing episode audio");

                        break;
                    }

                }
            }
            yield return null;
        }
        while (true)
        {
            if (!currentReEpisode.NoAnimation)
            {
                if (pump.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(currentReEpisode.AnimationName))
                {
                    Debug.Log("animation name is: " + currentReEpisode.AnimationName);
                }
                if (currentReEpisode.IdleAnimationName != "null")
                {
                    if (pump.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(currentReEpisode.IdleAnimationName))
                    {
                        Debug.Log("idle animation name is: " + currentReEpisode.IdleAnimationName);
                        break;
                    }
                }
                
            }
            else
            {
                break;
            }

            /*if (pumpAnimator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !pumpAnimator.GetComponent<Animator>().IsInTransition(0))
            {

                if (!audioSource.isPlaying)
                {
                    Debug.Log("animation and audio is finished");
                   


                    break;
                }

            }*/
            yield return null;
        }

        foreach (string name in currentReEpisode.TransparentObjectNames)
        {
            GameObject transpObject = GameObject.Find(name);

            if (transpObject.GetComponent<MeshRenderer>().materials.Count() > 0)
            {
                Material[] RegMatArray = new Material[2];
                RegMatArray[0] = RegularMaterial;
                RegMatArray[1] = RegularMaterial;
                transpObject.GetComponent<MeshRenderer>().materials = RegMatArray;
            }
            else
            {
                transpObject.GetComponent<MeshRenderer>().material = RegularMaterial;
            }

        }
        ReEpisodeObject.GetComponent<Collider>().enabled = false;
        ReEpisodeIndex += 1;
        Debug.Log("playing next episode, index is " + ReEpisodeIndex);
        if (ReEpisodeIndex < (PumpReEpisodes.Count))
        {
            PlayNextEpisode();
        }

        yield return null;

    }

    public void PlayIntroAudio()
    {
        audioSource.clip = introClip;
        audioSource.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayReEpisodeCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
