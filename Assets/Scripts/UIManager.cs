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
    public bool isPaused = false;
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

    public void PauseAudio()
    {
        if (isPaused)
        {
            isPaused = false;
            GameObject.Find("EventSystem").GetComponent<AudioSource>().Pause();
        }
        
        
    }

    public void PlayAudio()
    {
        if (!isPaused)
        {
            
                GameObject.Find("EventSystem").GetComponent<AudioSource>().UnPause();
            isPaused = true;
            
        }
    }

    public void CollapseMenu()
    {
        DOTween.To(() => GameObject.Find("BottomPanel").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("BottomPanel").GetComponent<RectTransform>().anchorMax = dest, new Vector2(1, 0.1662254f), 1);
        DOTween.To(() => GameObject.Find("TextFrame").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("TextFrame").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.989000022f, 0.0936454684f), 1);
        DOTween.To(() => GameObject.Find("SimScriptText").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("SimScriptText").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.975500643f, 0.08622545f), 1);

        DOTween.To(() => GameObject.Find("ExpandSubmenuButton").GetComponent<RectTransform>().anchorMin, dest => GameObject.Find("ExpandSubmenuButton").GetComponent<RectTransform>().anchorMin = dest, new Vector2(0, 0.103000008f), 1);
        DOTween.To(() => GameObject.Find("ExpandSubmenuButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("ExpandSubmenuButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.0370000005f, 0.152999997f), 1);
        DOTween.To(() => GameObject.Find("SoundOnButton").GetComponent<RectTransform>().anchorMin, dest => GameObject.Find("SoundOnButton").GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.94600004f, 0.103000008f), 1);
        DOTween.To(() => GameObject.Find("SoundOnButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("SoundOnButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.980500638f, 0.155000001f), 1);
        DOTween.To(() => GameObject.Find("BackButton").GetComponent<RectTransform>().anchorMin, dest => GameObject.Find("BackButton").GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.38860628f, 0.103000008f), 1);
        DOTween.To(() => GameObject.Find("BackButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("BackButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.422000021f, 0.155000001f), 1);
        DOTween.To(() => GameObject.Find("ForwardButton").GetComponent<RectTransform>().anchorMin, dest => GameObject.Find("ForwardButton").GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.533000052f, 0.103000008f), 1);
        DOTween.To(() => GameObject.Find("ForwardButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("ForwardButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.564999998f, 0.155000001f), 1);
        DOTween.To(() => GameObject.Find("TextSlider").GetComponent<RectTransform>().anchorMin, dest => GameObject.Find("TextSlider").GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.671000004f, 0.1121291f), 1);
        DOTween.To(() => GameObject.Find("TextSlider").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("TextSlider").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.791000009f, 0.152999997f), 1);
        DOTween.To(() => GameObject.Find("MenuButton").GetComponent<RectTransform>().anchorMin, dest => GameObject.Find("MenuButton").GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.0610000044f, 0.103000008f), 1);
        DOTween.To(() => GameObject.Find("MenuButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("MenuButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.0920000076f, 0.152999997f), 1);
        DOTween.To(() => GameObject.Find("PlayButton").GetComponent<RectTransform>().anchorMin, dest => GameObject.Find("PlayButton").GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.438000023f, 0.103000008f), 1);
        DOTween.To(() => GameObject.Find("PlayButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("PlayButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.471000016f, 0.155000001f), 1);
        DOTween.To(() => GameObject.Find("PauseButton").GetComponent<RectTransform>().anchorMin, dest => GameObject.Find("PauseButton").GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.480750859f, 0.103000008f), 1);
        DOTween.To(() => GameObject.Find("PauseButton").GetComponent<RectTransform>().anchorMax, dest => GameObject.Find("PauseButton").GetComponent<RectTransform>().anchorMax = dest, new Vector2(0.512000024f, 0.155000001f), 1);

        //ExpandSubmenuButton

        //Vector2(0.975500643,0.08622545)
    }
}
