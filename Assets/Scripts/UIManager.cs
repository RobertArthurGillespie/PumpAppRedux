using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI simText;
    public Slider textSlider;
    public bool menuIsOpen = false;
    public bool soundOff = false;
    public Sprite soundOnButtonImage;
    public Sprite soundOffButtonImage;
    public GameObject soundButton;
    public AudioClip openSoundClip;
    public AudioClip closeSoundClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTextSize()
    {
        simText.fontSize = textSlider.value;
    }

    public IEnumerator OpenMainMenuCoroutine()
    {
        GameObject.Find("TitratorScene").GetComponent<AudioSource>().clip = openSoundClip;
        GameObject.Find("TitratorScene").GetComponent<AudioSource>().Play();

        DOTween.To(() => GameObject.Find("MainMenuPanel").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("MainMenuPanel").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.661000013f, 0.810225546f), 1);
        yield return new WaitForSeconds(1f);
        GameObject.Find("HomeImage").GetComponent<Image>().enabled = true;
        GameObject.Find("SettingsImage").GetComponent<Image>().enabled = true;
        GameObject.Find("HomeText").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.Find("SettingsText").GetComponent<TextMeshProUGUI>().enabled = true;
        DOTween.To(() => GameObject.Find("HomeButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("HomeButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.61644882f, 0.764741898f), 0.5f);
        DOTween.To(() => GameObject.Find("SettingsButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("SettingsButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.61644882f, 0.5633873f), 0.5f);
        
        //Vector2(0.378842503,0.649000049)
    }

    public IEnumerator CloseMainMenuCoroutine()
    {
        GameObject.Find("TitratorScene").GetComponent<AudioSource>().clip = closeSoundClip;
        GameObject.Find("TitratorScene").GetComponent<AudioSource>().Play();
        DOTween.To(() => GameObject.Find("MainMenuPanel").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("MainMenuPanel").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.329000026f, 0.810225546f), 1);
        yield return new WaitForSeconds(1f);
        GameObject.Find("HomeImage").GetComponent<Image>().enabled = false;
        GameObject.Find("SettingsImage").GetComponent<Image>().enabled = false;
        GameObject.Find("HomeText").GetComponent<TextMeshProUGUI>().enabled = false;
        GameObject.Find("SettingsText").GetComponent<TextMeshProUGUI>().enabled = false;
        DOTween.To(() => GameObject.Find("HomeButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("HomeButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.378842503f, 0.764741898f), 0.5f);
        DOTween.To(() => GameObject.Find("SettingsButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("SettingsButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.378842503f, 0.563387275f), 0.5f);
        
        //Vector2(0.378842503,0.649000049)
    }
    public void OpenMainMenu()
    {
        menuIsOpen = true;
        StartCoroutine(OpenMainMenuCoroutine());
    }

    public void CloseMainMenu()
    {
        menuIsOpen = false;
        StartCoroutine(CloseMainMenuCoroutine());
    }

    public void ToggleMainMenu()
    {
        if(menuIsOpen)
        {
            CloseMainMenu();
        }
        else
        {
            OpenMainMenu();
        }
    }

    public void ToggleSound()
    {
        if (soundOff)
        {
            GameObject.Find("EventSystem").GetComponent<AudioSource>().mute = false;
            soundOff = false;
            soundButton.GetComponent<Image>().sprite = soundOnButtonImage;
        }
        else
        {
            soundOff = true;
            GameObject.Find("EventSystem").GetComponent<AudioSource>().mute = true;
            soundButton.GetComponent<Image>().sprite = soundOffButtonImage;
        }
    }
}
