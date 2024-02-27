using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PumpAssemblyManager : MonoBehaviour
{
    public GameObject pumpAnimator;
    public GameObject pump;
    public List<Episode> PumpEpisodes = new List<Episode>();
    public AudioSource audioSource;
    public int episodeIndex = 0;
    public AudioClip introClip;
    GameObject episodeObject;

    public void PlayEpisode()
	{
        StartCoroutine(PlayEpisodeCoroutine());
	}

    public IEnumerator PlayEpisodeCoroutine()
	{
        
        Episode currentEpisode = PumpEpisodes[episodeIndex];
        audioSource.clip = currentEpisode.EpisodeAudio;
        audioSource.Play();
        episodeObject = GameObject.Find(currentEpisode.EpisodeObjectName);
        while (true)
        {
            if (pump.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(currentEpisode.AnimationIdleStateName))
            {
                Debug.Log("in proper idle animation");
                break;
            }
            else
            {
                Debug.Log("not in proper idle animation yet");
            }
            yield return null;
        }
        episodeObject.GetComponent<MeshRenderer>().enabled = true;
        episodeObject.GetComponent<Collider>().enabled = true;
        //episodeObject.GetComponent<Outline>().enabled = true;
        episodeObject.AddComponent<Outline>();
        Outline outlineToEdit = episodeObject.GetComponent<Outline>();
        outlineToEdit.OutlineColor = new Color(1.60200012f, 1.57944012f, 0, 1);
        outlineToEdit.OutlineWidth = 8f;
        //episodeObject.GetComponent<MeshRenderer>().materials[1] = episodeObject.GetComponent<MeshRenderer>().materials[0];
        if (currentEpisode.AdditionalObjectNames.Count > 0)
        {
            foreach(string name in currentEpisode.AdditionalObjectNames)
            {
                GameObject objectToActivate = GameObject.Find(name);
                objectToActivate.GetComponent<MeshRenderer>().enabled = true;
                objectToActivate.AddComponent<Outline>();
                Outline subOutlineToEdit = objectToActivate.GetComponent<Outline>();
                subOutlineToEdit.OutlineColor = new Color(1.60200012f, 1.57944012f, 0, 1);
                subOutlineToEdit.OutlineWidth = 8f;
                if(objectToActivate.TryGetComponent<Collider>(out Collider coll))
                {
                    coll.enabled = true;
                }
                /*try
                {
                    objectToActivate.GetComponent<Collider>().enabled = true;
                }
                catch
                {
                    Debug.Log("this one doesn't have a collider");
                }*/
                

            }
        }
       

        while (true)
		{
            if (Input.GetMouseButtonDown(0))
            {



                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))

                {


                    Debug.Log(hit.transform.name);


                    Debug.Log("hit");
                    if (hit.transform.tag == currentEpisode.CorrectObjectTag)
                    {
                        
                        episodeObject.GetComponent<Outline>().enabled = false;
                        
                        if (currentEpisode.AdditionalObjectNames.Count > 0)
                        {
                            foreach(string name in currentEpisode.AdditionalObjectNames)
                            {
                                Debug.Log("name of additional object is: " + name);
                                GameObject subObject = GameObject.Find(name);
                                subObject.AddComponent<Outline>();
                                Outline subOutlineToEdit = episodeObject.GetComponent<Outline>();
                                subOutlineToEdit.OutlineColor = new Color(1.60200012f, 1.57944012f, 0, 1);
                                subOutlineToEdit.OutlineWidth = 8f;
                                subObject.GetComponent<Outline>().enabled = false;
                            }
                        }
                        pumpAnimator.GetComponent<Animator>().SetBool(currentEpisode.AnimationBool,true);
                        Debug.Log("move to next step");
                        
                        break;
                    }

                }
            }
            yield return null;
		}
        while (true)
        {
            if (pump.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !pump.GetComponent<Animator>().IsInTransition(0))
            {
                
                if (!audioSource.isPlaying)
                {
                    Debug.Log("animation and audio is finished");
                    episodeIndex += 1;

                    
                    break;
                }
                
            }
                yield return null;
        }
        if (episodeIndex <= 6)
        {
            PlayEpisode();

        }
        

    }

    public IEnumerator PlayIntro()
    {
        audioSource.clip = introClip;
        audioSource.Play();
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                PlayEpisode();
                break;
            }
            yield return null;
        }
    }

    public void HideObjectsOnStart()
    {
        MeshRenderer[] renderers = pump.GetComponentsInChildren<MeshRenderer>();
        Collider[] colliders = pump.GetComponentsInChildren<Collider>();
        foreach (MeshRenderer r in renderers)
        {
            if (r.gameObject.name != "FloorMount.001")
            {
                r.enabled = false;
            }

        }
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //HideObjectsOnStart();
        //PlayEpisode();
        //StartCoroutine(PlayIntro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
