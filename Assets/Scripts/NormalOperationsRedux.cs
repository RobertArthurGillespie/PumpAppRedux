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
    public PointerState pointerState;
    public GameObject ValveWheelDischarge;
    public GameObject ValveWheelSuction;
    public int lerpProgress = 0;
    public bool isLerping = false;
    public bool dischargeSelected = false;
    public bool suctionSelected = false;
    public float initialValveValue = 0f;
    public float endValveValue = 0f;
    public void PlaySuccesAudio()
    {
        if (pointerState.pointerIsUp)
        {
            Debug.Log("pointer is up");
            if (!isLerping)
            {
                endValveValue = slider.value;
                Debug.Log("starting animations, endvalvevalue is: "+endValveValue);
                StartCoroutine(ScheduleLerp());
                StartCoroutine(RotateSuctionValve());
            }
            
            //run valve animation
        }
        if (slider.value > successValueLowerBound&&slider.value<successValueUpperBound)
        {
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
            successClip = RecirculationSuccessClip;
            audioSource.clip = RecirculationScenarioClip;
            audioSource.Play();
            RunScenario();
        }
        else if (isHighDischarge)
        {
            CurrentScenario = NormalOperationsScenarios.HighDischarge;
            successClip = HighDischargeSuccessClip;
            audioSource.clip = HighDischargeScenarioClip;
            audioSource.Play();
            RunScenario();
        }
        else if (isLowSuction)
        {
            CurrentScenario = NormalOperationsScenarios.LowSuction;
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
        while (true)
        {
            if (lerpProgress < 11)
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
        Debug.Log("pointer up valve value: " + pointerState.pointerUpValveValue + " and initial valuve value: " + initialValveValue);
        while (true)
        {
            if (lerpProgress < 10)
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
            else if (lerpProgress == 10)
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
