using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BioreactorReduxManager : MonoBehaviour
{
    public GameObject bioAnimator;
    public GameObject bio;
    public GameObject bioHighlight;
    public GameObject fieldHighlight;
    public GameObject woodChipParticles;
    public List<ReEpisode> bioReEpisodes = new List<ReEpisode>();
    public AudioSource audioSource;
    public int ReEpisodeIndex = 0;
    public AudioClip introClip;
    public Material RegularMaterial;
    public Material TransMaterial;
    public Material HighlightMaterial;
    public bool introAudioFinished = false;
    GameObject ReEpisodeObject;
    public bool endAllGlowCycles = false;
    public Material glowMat;
    public Color BeginColor;
    public Color EndColor;
    public GameObject VCam1;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(PlayReEpisodeCoroutine());
        StartCoroutine(PlayIntroGlowAndParticles());
        StartCoroutine(ScheduleIntroGlowCycle());

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator PlayIntroGlowAndParticles()
    {
        //start glow 255,242,0,255
        //end glow 82, 78, 0, 255
        Debug.Log("starting intro sequence, endAllGlowCycles is: "+endAllGlowCycles);
        audioSource.clip = introClip;
        audioSource.Play();
        woodChipParticles.GetComponent<ParticleSystem>().Play();
        bool endGlowCycle1 = false;
        bool endGlowCycle2 = false;
        //Color beginColor = new Color(106f, 100f, 0f);
        //Color endColor = new Color(255f, 241f, 0f);
        //Color lerpColor = new Color();
        float t = 0f;
        while (true)
        {
            if (!endAllGlowCycles)
            {
                if (!endGlowCycle1)
                {
                    Color lerpColor = new Color();
                    lerpColor = Color.Lerp(BeginColor, EndColor, t);
                    
                    Debug.Log("lerp color is: " + lerpColor.r + "," + lerpColor.g + "," + lerpColor.b);
                    Debug.Log("t is: " + t);
                    //Debug.Log("main color of biohighlight is: "+)
                    /*Material bioHighlightMaterial = bioHighlight.GetComponent<MeshRenderer>().material;
                    Color biohighlightColor = bioHighlightMaterial.color;
                    biohighlightColor = lerpColor;
                    bioHighlightMaterial.color = biohighlightColor;*/
                    //glowMat.SetColor("_BaseColor",lerpColor);
                    Material glowM = bioHighlight.GetComponent<Renderer>().material;
                    Material glowM2 = fieldHighlight.GetComponent<Renderer>().material;
                    glowM.SetColor("_BaseColor", lerpColor);
                    glowM2.SetColor("_BaseColor", lerpColor);
                    bioHighlight.GetComponent<Renderer>().material = glowM;
                    fieldHighlight.GetComponent<Renderer>().material = glowM2;
                    Debug.Log("biohighlightMaterial color is: " + glowM.color.r + "," + glowM.color.g + "," + glowM.color.b+", r is: "+bioHighlight.GetComponent<Renderer>().material.color.r);
                    
                    //fieldHighlight.GetComponent<MeshRenderer>().material.color = lerpColor;
                    t += .07f;
                    if (t >= 1f)
                    {
                        Debug.Log("ending glow cycle 1");
                        endGlowCycle1 = true;
                        endGlowCycle2 = false;
                        t = 0f;
                    }
                }
                else if (!endGlowCycle2)
                {
                    Color lerpColor = new Color();
                    lerpColor = Color.Lerp(EndColor, BeginColor, t);
                    Material glowM = bioHighlight.GetComponent<Renderer>().material;
                    Material glowM2 = fieldHighlight.GetComponent<Renderer>().material;
                    glowM.SetColor("_BaseColor", lerpColor);
                    glowM2.SetColor("_BaseColor", lerpColor);
                    bioHighlight.GetComponent<Renderer>().material = glowM;
                    fieldHighlight.GetComponent<Renderer>().material = glowM2;
                    Debug.Log("biohighlightMaterial color in cycle 2 is: " + glowM.color.r + "," + glowM.color.g + "," + glowM.color.b + ", r is: " + bioHighlight.GetComponent<Renderer>().material.color.r);

                    //fieldHighlight.GetComponent<MeshRenderer>().material.color = lerpColor;
                    t += .07f;
                    if (t >= 1f)
                    {
                        Debug.Log("resetting glow cycle");
                        endGlowCycle2 = true;
                        endGlowCycle1 = false;
                        t = 0f;
                    }
                }
            }
            else
            {
                woodChipParticles.GetComponent<ParticleSystem>().Stop();
                break;
            }
            
            
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

    public void PlayNextEpisode()
    {
        StartCoroutine(PlayReEpisodeCoroutine());
    }

    public IEnumerator PlayReEpisodeCoroutine()
    {
        

        if (!introAudioFinished)
        {

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
        ReEpisode currentReEpisode = bioReEpisodes[ReEpisodeIndex];
        if (currentReEpisode.OpeningExtensionMethods.Count > 0)
        {
            foreach(string method in currentReEpisode.OpeningExtensionMethods)
            {
                RunExtensionMethods(method);
            }
        }
        ReEpisodeObject = GameObject.Find(currentReEpisode.EpisodeObjectName);
        ReEpisodeObject.GetComponent<Collider>().enabled = true;
        RegularMaterial = ReEpisodeObject.GetComponent<MeshRenderer>().material;
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

        Material[] MatArray = ReEpisodeObject.GetComponent<MeshRenderer>().materials;
        MatArray[1] = HighlightMaterial;
        ReEpisodeObject.GetComponent<MeshRenderer>().materials = MatArray;
        if (currentReEpisode.AdditionalObjectNames.Count > 0)
        {
            foreach (string name in currentReEpisode.AdditionalObjectNames)
            {
                GameObject obj = GameObject.Find(name);
                Material[] GlowMatArray = obj.GetComponent<MeshRenderer>().materials;
                GlowMatArray[1] = HighlightMaterial;
                obj.GetComponent<MeshRenderer>().materials = GlowMatArray;
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
                        MatArray[1] = RegularMaterial;
                        ReEpisodeObject.GetComponent<MeshRenderer>().materials = MatArray;
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

                        if (currentReEpisode.OtherAnimator)
                        {
                            GameObject.Find(currentReEpisode.OtherAnimatorName).GetComponent<Animator>().SetBool(currentReEpisode.AnimationBool, true);
                        }
                        else if (!currentReEpisode.NoAnimation)
                        {
                            bioAnimator.GetComponent<Animator>().SetBool(currentReEpisode.AnimationBool, true);
                        }

                        audioSource.clip = currentReEpisode.EpisodeAudio;
                        audioSource.Play();
                        endAllGlowCycles = true;

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
                
                if (bio.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(currentReEpisode.AnimationName))
                {
                    Debug.Log("animation name is: " + currentReEpisode.AnimationName);
                }
                if (bio.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(currentReEpisode.IdleAnimationName))
                {
                    Debug.Log("animation name is: " + currentReEpisode.IdleAnimationName);
                    break;
                }
            }
            else
            {
                break;
            }

            /*if (bioAnimator.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !bioAnimator.GetComponent<Animator>().IsInTransition(0))
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
        if (currentReEpisode.ExtensionMethods.Count > 0)
        {
            foreach(string method in currentReEpisode.ExtensionMethods)
            {
                RunExtensionMethods(method);
            }
        }
        if (ReEpisodeIndex < (bioReEpisodes.Count))
        {
            while (true)
            {
                if (audioSource.isPlaying)
                {
                    Debug.Log("waiting to play next episode");
                }
                else
                {
                    break;
                }
                
                yield return null;
            }
            PlayNextEpisode();
        }

        yield return null;

    }

    public void PlayIntroAudio()
    {
        audioSource.clip = introClip;
        audioSource.Play();
    }

    public void RunExtensionMethods(string currentEpisodeMethod)
    {
        switch (currentEpisodeMethod)
        {
            case "RunHydrogenParticleSystem":
                Debug.Log("running particle system");
                RunHydrogenParticleSystem();
                break;

            case "ActivateVCam1":
                Debug.Log("activating VCam1");
                ActivateVCam1();
                break;

            case "TurnOffFieldHighlight":
                TurnOffFieldHighlight();
                break;
            case "ScaleWoodChipWaterDown":
                StartCoroutine(ScaleWoodchipWaterDown());
                break;

            case "ActivateVcam2":
                StartCoroutine(ActivateVcam2());
                break;


            default:
                Debug.Log("no methods run");
                break;
        }
        
    }

    public IEnumerator ActivateVcam2()
    {
        Debug.Log("activating Vcam2");
        GameObject.Find("VCam2").GetComponent<CinemachineVirtualCamera>().enabled = true;
        yield return new WaitForSeconds(35f);
        GameObject.Find("VCam2").GetComponent<CinemachineVirtualCamera>().enabled = false;
    }

    public IEnumerator ScaleWoodchipWaterDown()
    {
        Debug.Log("beginning scale down");
        yield return new WaitForSeconds(45f);
        GameObject.Find("WoodChips_Water").GetComponent<Animator>().SetBool("ScaleDown", true);
    }

    public void TurnOffFieldHighlight()
    {
        GameObject.Find("highlight_map").SetActive(false);
    }

    public void ActivateVCam1()
    {
        //VCam1.GetComponent<CinemachineVirtualCamera>().enabled = true;
        StartCoroutine(ActivateVCam1Coroutine());
    }

    public IEnumerator ActivateVCam1Coroutine()
    {
        VCam1.GetComponent<CinemachineVirtualCamera>().enabled = true;
        while (true)
        {
            if(!audioSource.isPlaying)
            {
                break;
            }
            else
            {
                Debug.Log("waiting to activate VCam");
            }
            yield return null;
        }
        VCam1.GetComponent<CinemachineVirtualCamera>().enabled = false;
    }

    public IEnumerator ScheduleIntroGlowCycle()
    {
        yield return new WaitForSeconds(10f);
        endAllGlowCycles = true;
        bioAnimator.GetComponent<Animator>().SetBool("OpenFieldView", true);
        TurnOffFieldHighlight();
        RunBioReactorSim();
    }

    public void RunBioReactorSim()
    {
        
        StartCoroutine(PlayReEpisodeCoroutine());
    }
    public void RunHydrogenParticleSystem()
    {

    }
}
