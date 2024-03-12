using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NormalOperationsRedux : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip successClip;
    public AudioClip introClip_1;
    public AudioClip introClip_2;
    public AudioClip cavitationClip;
    public float successValueLowerBound;
    public float successValueUpperBound;
    public Slider slider;
    public NormalOperationsScenarios CurrentScenario;
    public bool isRecirculation = false;
    public bool isHighDischarge = false;
    public bool isLowSuction = false;
    public AudioClip HighDischargeSuccessClip;
    public AudioClip LowSuctionSuccessClip;
    public AudioClip RecirculationSuccessClip;
    public AudioClip HighDischargeScenarioClip;
    public AudioClip LowSuctionScenarioClip;
    public AudioClip RecirculationScenarioClip;
    public TextMeshProUGUI valveLabel;
    public TextMeshProUGUI scenarioLabel;
    public PointerState pointerState;
    public GameObject ValveWheelDischarge;
    public GameObject ValveWheelSuction;
    public int lerpProgress = 0;
    public int lerpLimit = 11;
    public bool isLerping = false;
    public bool dischargeSelected = false;
    public bool suctionSelected = false;
    public float initialValveValue = 0f;
    public float endValveValue = 0f;
    public void PlaySuccesAudio()
    {
        
        if (slider.value > successValueLowerBound&&slider.value<successValueUpperBound)
        {
            if (pointerState.pointerIsUp)
            {
                Debug.Log("pointer is up");
                if (!isLerping)
                {
                    endValveValue = slider.value;
                    Debug.Log("starting animations, endvalvevalue is: " + endValveValue);
                    StartCoroutine(ScheduleLerp());
                    StartCoroutine(RotateSuctionValve());
                }

                //run valve animation
            }
            audioSource.clip = successClip;
            audioSource.Play();
            if (CurrentScenario == NormalOperationsScenarios.Recirculation)
            {
                isRecirculation = false;
            }
            else if (CurrentScenario == NormalOperationsScenarios.LowSuction)
            {
                isLowSuction = false;
            }
            else if (CurrentScenario == NormalOperationsScenarios.HighDischarge)
            {
                isHighDischarge = false;
            }
            IntroduceScenario();
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartNormalOperations());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialValveValue = slider.value;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag == "Discharge")
                {
                    valveLabel.gameObject.SetActive(true);
                    slider.gameObject.SetActive(true);
                    valveLabel.text = "Discharge";
                    dischargeSelected = true;
                    suctionSelected = false;
                }
                else if (hit.transform.tag == "Suction")
                {
                    valveLabel.gameObject.SetActive(true);
                    slider.gameObject.SetActive(true);
                    valveLabel.text = "Intake";
                    suctionSelected=true;
                    dischargeSelected=false;
                }
            }
        }
    }

    public IEnumerator StartNormalOperations()
    {
        audioSource.clip = introClip_1;
        audioSource.Play();
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                break;
            }
            yield return null;
        }
        audioSource.clip = cavitationClip;
        audioSource.Play();
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                break;
            }
            yield return null;
        }
        audioSource.clip = introClip_2;
        audioSource.Play();
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                break;
            }
            yield return null;
        }

        IntroduceScenario();
    }

    public void IntroduceScenario()
    {
        if (isRecirculation)
        {
            CurrentScenario = NormalOperationsScenarios.Recirculation;
            scenarioLabel.text = "Scenario: Recirculation";
            successClip = RecirculationSuccessClip;
            audioSource.clip = RecirculationScenarioClip;
            audioSource.Play();
            RunScenario();
        }
        else if (isHighDischarge)
        {
            CurrentScenario = NormalOperationsScenarios.HighDischarge;
            scenarioLabel.text = "Scenario: High Discharge";
            successClip = HighDischargeSuccessClip;
            audioSource.clip = HighDischargeScenarioClip;
            audioSource.Play();
            RunScenario();
        }
        else if (isLowSuction)
        {
            CurrentScenario = NormalOperationsScenarios.LowSuction;
            scenarioLabel.text = "Scenario: Low Suction";
            successClip = LowSuctionSuccessClip;
            audioSource.clip = LowSuctionScenarioClip;
            audioSource.Play();
            RunScenario();
        }
    }

    public void RunScenario()
    {
        switch (CurrentScenario)
        {
            case NormalOperationsScenarios.HighDischarge:
                Debug.Log("In high discharge");
                successValueLowerBound = 0.0f;
                successValueUpperBound = 0.4f;
                break;

            case NormalOperationsScenarios.LowSuction:
                Debug.Log("In low suction");
                successValueLowerBound = 0.6f;
                successValueUpperBound = 1.0f;
                break;

            case NormalOperationsScenarios.Recirculation:
                Debug.Log("in recirculation");
                successValueLowerBound = 0.6f;
                successValueUpperBound = 1.0f;
                break;
        }
    }

    public IEnumerator ScheduleLerp()
    {
        lerpProgress = 0;
        float sliderChangeDistance = 0f;
        sliderChangeDistance = initialValveValue -= endValveValue;
        float absoluteSliderChangeDist = Mathf.Abs(sliderChangeDistance);
        Debug.Log("slider change distance is: " + absoluteSliderChangeDist);
        if(absoluteSliderChangeDist > 0.5f)
        {
            lerpLimit = 8;
        }
        else if (absoluteSliderChangeDist < 0.25f)
        {
            lerpLimit = 3;
        }
        Debug.Log("Lerp limit is: " + lerpLimit);
        while (true)
        {
            if (lerpProgress < lerpLimit)
            {
                lerpProgress++;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
            yield return null;
        }
        Debug.Log("lerp finished, resetting progress and lerp bool");
        lerpProgress = 0;
        isLerping = false;
        yield return null;
    }
    public IEnumerator RotateSuctionValve()
    {
        isLerping = true;
        //start lifetime 2.22, (originally 6.22), start speed set to 10.64 (originally 5.64)
        if (suctionSelected)
        {
            if (endValveValue > 0.5f)
            {
                GameObject.Find("Flow_Bubbles_Suction").GetComponent<ParticleSystem>().startLifetime = 3.22f;
                GameObject.Find("Flow_Bubbles_Suction").GetComponent<ParticleSystem>().startSpeed = 10.64f;
            }
            else if (endValveValue < 0.5f)
            {
                GameObject.Find("Flow_Bubbles_Suction").GetComponent<ParticleSystem>().startLifetime = 6.22f;
                GameObject.Find("Flow_Bubbles_Suction").GetComponent<ParticleSystem>().startSpeed = 5.64f;
            }
            
        }
        else if (dischargeSelected)
        {
            if (endValveValue > 0.5f)
            {
                GameObject.Find("Flow_Bubbles_Discharge").GetComponent<ParticleSystem>().startLifetime = 3.22f;
                GameObject.Find("Flow_Bubbles_Discharge").GetComponent<ParticleSystem>().startSpeed = 10.64f;
            }
            else if (endValveValue < 0.5f)
            {
                GameObject.Find("Flow_Bubbles_Discharge").GetComponent<ParticleSystem>().startLifetime = 6.22f;
                GameObject.Find("Flow_Bubbles_Discharge").GetComponent<ParticleSystem>().startSpeed = 5.64f;
            }
            
        }
        while (true)
        {
            if (lerpProgress < lerpLimit)
            {
                if (suctionSelected)
                {
                    if (endValveValue > initialValveValue)
                    {
                        ValveWheelSuction.transform.Rotate(0f, 0f, -1f);
                    }
                    else if(endValveValue < initialValveValue)
                    {
                        ValveWheelSuction.transform.Rotate(0f, 0f, 1f);
                    }


                }
                else if (dischargeSelected)
                {
                    Debug.Log("endValveValue is: " + endValveValue + ", initialValveValue is: " + initialValveValue);
                    if(endValveValue > initialValveValue)
                    {
                        ValveWheelDischarge.transform.Rotate(0f, 0f, -1f);

                    }
                    else if(endValveValue < initialValveValue)
                    {
                        ValveWheelDischarge.transform.Rotate(0f, 0f, 1f);
                    }
                    
                }
                
            }
            else if (lerpProgress >= lerpLimit)
            {
                isLerping = false;
                break;
                
            }
            yield return null;
        }
        

        yield return null;
    }
}

public enum NormalOperationsScenarios { HighDischarge, LowSuction, Recirculation}
