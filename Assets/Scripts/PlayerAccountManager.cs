using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerAccountManager : MonoBehaviour
{
    //use to access user profile information
    public UserProfile userProfile;
    //use to access user's stats in this sim
    public UserSimStats userStats;

    //To get sim Id from the products table
    /*[DllImport("__Internal")]
    private static extern string GetSimId();*/

    //to save changes to user's stats
    public TextMeshProUGUI userTextbox;
    public GameObject userIconPanel;
    public Image OpenMenuArrow;
    public GameObject minimizeButton;
    public GameObject openUserMenuButton;
    public Image userImage;
    public Sprite closePanelSprite;
    public bool panelIsOpen = true;
    public bool chatIsOpen = false;

    //replace this with the given number matching the sim's id in the products table
    public string simId = "0";
    public string simIdTest = "";
    public ChatManager chatManager;

    //chat UI stuff
    public GameObject chatPanel;
    public GameObject chatPanelText;
    public GameObject chatInputField;
    public GameObject chatInputFieldText;
    public GameObject connectButton;
    public GameObject sendButton;
    public GameObject connectButtonText;
    public GameObject sendButtonText;
    public GameObject closeChatButton;

    //To customize animations
    public Vector2 closeChatPanelDestination;
    public Vector2 closeChatInputFieldDestination;
    public Vector2 closeSendButtonDestination;
    public Vector2 openChatPanelDestination;
    public Vector2 openChatInputFieldDestination;
    public Vector2 openSendButtonDestination;

    public Vector2 initChatPanelValue;

    public IEnumerator AddToUserRep(int amount)
    {
        string amountString = amount.ToString();
        UnityWebRequest www = UnityWebRequest.Get("https://avrestore.azurewebsites.net/api/avr/AddToUserRep?amount=" + amountString);
        Debug.Log("adding to user rep at: " + www.url);
        yield return www.SendWebRequest();
        while (true)
        {
            if (www.isDone)
            {
                Debug.Log("rep should be added to, response is: " + www.result);
                break;
            }
            else if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("something went wrong with the connection");
                break;
            }
            yield return null;
        }
    }

    public IEnumerator SubtractFromUserRep(string amount)
    {
        string amountString = amount.ToString();
        UnityWebRequest www = UnityWebRequest.Get("https://avrestore.azurewebsites.net/api/avr/SubtractFromUserRep?amount=" + amountString);
        Debug.Log("subtracting from user rep at: " + www.url);
        yield return www.SendWebRequest();
        while (true)
        {
            if (www.isDone)
            {
                Debug.Log("rep should be added to, response is: " + www.result);
                break;
            }
            else if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("something went wrong with the connection");
                break;
            }
            yield return null;
        }
    }

    public IEnumerator MarkSimCompleted(bool hasFinished)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://avrestore.azurewebsites.net/api/avr/SetSimCompletion?simId=" + chatManager.currentSimId + "&hasCompleted=" + hasFinished);
        Debug.Log("subtracting from user rep at: " + www.url);
        yield return www.SendWebRequest();
        while (true)
        {
            if (www.isDone)
            {
                Debug.Log("rep should be added to, response is: " + www.result);
                break;
            }
            else if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("something went wrong with the connection");
                break;
            }
            yield return null;
        }
    }
    public IEnumerator GetPlayerInfoCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://avrestore.azurewebsites.net/api/avr/GetUserProfileForUnity");
        Debug.Log("Url is: " + www.url);
        yield return www.SendWebRequest();

        while (true)
        {
            if (www.isDone)
            {
                Debug.Log("success in Json!  downloadhandler is: " + www.downloadHandler.text);
                char[] trimArray = { '[', ']' };
                string trimmedJson = www.downloadHandler.text.Trim(trimArray);
                Debug.Log("trimmed downloadhandler is: " + trimmedJson);
                //User theUser = User.CreateFromJSON(jsonGet.downloadHandler.text);
                //User theUser = User.CreateFromJSON(trimmedJson);
                //User theUser = JsonUtility.FromJson<User>("{\"root\":" + www.downloadHandler.text + "}");
                userProfile = JsonUtility.FromJson<UserProfile>(trimmedJson);
                Debug.Log("the user's primary id is: " + userProfile.id + " and their userID is: " + userProfile.userId + "and their username is: " + userProfile.userName + "and their icon is: " + userProfile.icon);
                break;
            }
            else if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("something went wrong in json");
                break;
            }
            yield return null;
        }
        if (userProfile.icon.Length > 1)
        {
            StartCoroutine(RenderUserIcon(userProfile.icon));
            if (userProfile.userName.Length > 0)
            {
                userTextbox.text = userProfile.userName;
            }

        }
        else
        {
            Debug.Log("no user icon");
        }

        yield return null;
    }

    public IEnumerator GetPlayerStatsCoroutine()
    {
        while (true)
        {
            if (chatManager.currentSimId != "0")
            {
                Debug.Log("sim Id is: " + chatManager.currentSimId);
                break;
            }
            yield return null;
        }
        UnityWebRequest www = UnityWebRequest.Get("https://avrestore.azurewebsites.net/api/avr/GetUserSimStatsForUnity?simId=" + simId);
        Debug.Log("Url is: " + www.url);
        yield return www.SendWebRequest();

        while (true)
        {
            if (www.isDone)
            {
                Debug.Log("success in Json!  downloadhandler is: " + www.downloadHandler.text);
                char[] trimArray = { '[', ']' };
                string trimmedJson = www.downloadHandler.text.Trim(trimArray);
                Debug.Log("trimmed downloadhandler is: " + trimmedJson);
                //User theUser = User.CreateFromJSON(jsonGet.downloadHandler.text);
                //User theUser = User.CreateFromJSON(trimmedJson);
                //User theUser = JsonUtility.FromJson<User>("{\"root\":" + www.downloadHandler.text + "}");
                userStats = JsonUtility.FromJson<UserSimStats>(trimmedJson);
                Debug.Log("the user's score is: " + userStats.score + " and their completion boolean is: " + userStats.hasCompleted + " and their user Id is: " + userStats.userId);
                break;
            }
            else if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("something went wrong in json");
                break;
            }
            yield return null;
        }


        yield return null;
    }

    public void SendUserEmail(string subject, string message)
    {
        StartCoroutine(SendUserEmailCoroutine(subject, message));
    }

    public IEnumerator SendUserEmailCoroutine(string subject, string message)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://avrestore.azurewebsites.net/api/avr/SendUserEmail?subject=" + subject + "&message=" + message);
        Debug.Log("Url is: " + www.url);
        yield return www.SendWebRequest();
        while (true)
        {
            if (www.isDone)
            {
                Debug.Log("Success; the user's subject is: " + subject + " and their message is : " + message);

                break;
            }
            else if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("something went wrong in json");
                break;
            }
            yield return null;
        }
    }

    public void GetPlayerStats()
    {
        StartCoroutine(GetPlayerStatsCoroutine());
    }

    public void PopulateUserUI()
    {

    }

    public void TogglePanel()
    {
        if (panelIsOpen)
        {
            CloseUserIconPanel();
            panelIsOpen = !panelIsOpen;
        }
        else if (!panelIsOpen)
        {
            OpenUserIconPanel();
            panelIsOpen = !panelIsOpen;
        }
    }

    public IEnumerator RenderUserIcon(string iconPath)
    {
        if (iconPath.Contains("blob"))
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(iconPath);
            yield return request.SendWebRequest();
            while (true)
            {
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("something went wrong in getting texture");
                    break;
                }
                else if (request.isDone)
                {
                    Texture2D downloadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    Sprite downloadedSprite = Sprite.Create(downloadedTexture, new Rect(0, 0, downloadedTexture.width, downloadedTexture.height), new Vector2(downloadedTexture.width / 2, downloadedTexture.height / 2));
                    GameObject.Find("UserIcon").GetComponent<Image>().overrideSprite = downloadedSprite;
                    break;
                }
                yield return null;
            }
        }
        else
        {
            Debug.Log("icon path is faulty");
        }
        yield return null;
    }

    public void CloseUserIconPanel()
    {
        Debug.Log("closing panel");
        openUserMenuButton.GetComponent<Image>().enabled = false;
        userTextbox.enabled = false;
        userImage.enabled = false;
        Vector2 OpenPanelAnchorPos = userIconPanel.GetComponent<RectTransform>().anchorMin;
        DOTween.To(() => userIconPanel.GetComponent<RectTransform>().anchorMin, dest => userIconPanel.GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.960283041f, 0.918322146f), 1);
        minimizeButton.GetComponent<Button>().image.sprite = OpenMenuArrow.sprite;
        openUserMenuButton.SetActive(false);
        Debug.Log("new anchor min" + OpenPanelAnchorPos.x + "," + OpenPanelAnchorPos.y);
    }

    public void OpenUserIconPanel()
    {
        StartCoroutine(ReOpenUserPanel());
        DOTween.To(() => userIconPanel.GetComponent<RectTransform>().anchorMin, dest => userIconPanel.GetComponent<RectTransform>().anchorMin = dest, new Vector2(0.838000059f, 0.697614312f), 1);
    }

    public IEnumerator ReOpenUserPanel()
    {
        yield return new WaitForSeconds(1.2f);
        openUserMenuButton.SetActive(true);
        openUserMenuButton.GetComponent<Image>().enabled = true;
        minimizeButton.GetComponent<Image>().sprite = closePanelSprite;
        userImage.enabled = true;
        userTextbox.enabled = true;
        yield return null;
    }

    public void CloseChatArea()
    {
        //do animations for InputField,Textbox, Send Button (everything but connect button)
        DOTween.To(() => chatPanel.GetComponent<RectTransform>().anchorMax, dest => chatPanel.GetComponent<RectTransform>().anchorMax = dest, closeChatPanelDestination, 1);
        Color textColor = chatPanelText.GetComponent<TextMeshProUGUI>().color;
        chatPanelText.GetComponent<TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, 0f);
        closeChatButton.SetActive(false);
        DOTween.To(() => chatInputField.GetComponent<RectTransform>().anchorMax, dest => chatInputField.GetComponent<RectTransform>().anchorMax = dest, closeChatInputFieldDestination, 1);
        DOTween.To(() => sendButton.GetComponent<RectTransform>().anchorMax, dest => sendButton.GetComponent<RectTransform>().anchorMax = dest, closeSendButtonDestination, 1);
        Color sendButtonTextColor = sendButtonText.GetComponent<TextMeshProUGUI>().color;
        sendButtonText.GetComponent<TextMeshProUGUI>().color = new Color(sendButtonTextColor.r, sendButtonTextColor.g, sendButtonTextColor.b, 0f);
        chatIsOpen = false;
    }

    public void OpenChatArea()
    {
        if (!chatIsOpen)
        {
            //do animations for InputField,Textbox, Send Button (everything but connect button)
            DOTween.To(() => chatPanel.GetComponent<RectTransform>().anchorMax, dest => chatPanel.GetComponent<RectTransform>().anchorMax = dest, /*new Vector2(0.321714312f, 0.259000003f)*/openChatPanelDestination, 1);
            StartCoroutine(WaitAndEnableChatText());

            DOTween.To(() => chatInputField.GetComponent<RectTransform>().anchorMax, dest => chatInputField.GetComponent<RectTransform>().anchorMax = dest, /*new Vector2(0.554857194f, 0.292000026f)*/openChatInputFieldDestination, 1);
            DOTween.To(() => sendButton.GetComponent<RectTransform>().anchorMax, dest => sendButton.GetComponent<RectTransform>().anchorMax = dest, /*new Vector2(0.436000019f, 0.128000006f)*/openSendButtonDestination, 1);
            Color sendButtonTextColor = sendButtonText.GetComponent<TextMeshProUGUI>().color;
            sendButtonText.GetComponent<TextMeshProUGUI>().color = new Color(sendButtonTextColor.r, sendButtonTextColor.g, sendButtonTextColor.b, 1f);
            //closeChatButton.SetActive(true);
            StartCoroutine(WaitAndEnable(closeChatButton));
            chatIsOpen = true;
        }


    }

    public IEnumerator WaitAndEnable(GameObject g)
    {
        yield return new WaitForSeconds(1f);
        g.SetActive(true);
    }

    public IEnumerator WaitAndEnableChatText()
    {
        yield return new WaitForSeconds(1f);
        Color textColor = chatPanelText.GetComponent<TextMeshProUGUI>().color;
        chatPanelText.GetComponent<TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, 1f);
    }

    public void InitChatArea()
    {
        DOTween.To(() => chatPanel.GetComponent<RectTransform>().anchorMax, dest => chatPanel.GetComponent<RectTransform>().anchorMax = dest, closeChatPanelDestination,0);//new Vector2(0f, 0f), 0);
        Color textColor = chatPanelText.GetComponent<TextMeshProUGUI>().color;
        chatPanelText.GetComponent<TextMeshProUGUI>().color = new Color(textColor.r, textColor.g, textColor.b, 0f);
        closeChatButton.SetActive(false);
        DOTween.To(() => chatInputField.GetComponent<RectTransform>().anchorMax, dest => chatInputField.GetComponent<RectTransform>().anchorMax = dest, closeChatInputFieldDestination, 0);
        DOTween.To(() => sendButton.GetComponent<RectTransform>().anchorMax, dest => sendButton.GetComponent<RectTransform>().anchorMax = dest, closeSendButtonDestination, 0);
        Color sendButtonTextColor = sendButtonText.GetComponent<TextMeshProUGUI>().color;
        sendButtonText.GetComponent<TextMeshProUGUI>().color = new Color(sendButtonTextColor.r, sendButtonTextColor.g, sendButtonTextColor.b, 0f);
        chatIsOpen = false;
    }

    public void GetPlayerInfo()
    {
        StartCoroutine(GetPlayerInfoCoroutine());
    }
    // Start is called before the first frame update
    void Start()
    {
        closePanelSprite = minimizeButton.GetComponent<Image>().sprite;
        GetPlayerInfo();
        GetPlayerStats();
        InitChatArea();
        //Debug.Log(userIconPanel.GetComponent<RectTransform>().anchorMin.x + ", " + userIconPanel.GetComponent<RectTransform>().anchorMin.y+", max: "+ userIconPanel.GetComponent<RectTransform>().anchorMax.x+","+ userIconPanel.GetComponent<RectTransform>().anchorMax.y);
        //Vector2(0.838000059,0.697614312)
        //shrunk: Vector2(0.960283041,0.918322146)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class UserProfile
{
    public int id;
    public int userId;
    public string userName;
    public string icon;
}

[System.Serializable]
public class UserSimStats
{
    public int userId;
    public int productId;
    public bool hasCompleted;
    public int score;
}
