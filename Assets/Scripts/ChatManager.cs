using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public TextMeshProUGUI MessageWindow;

    public TMP_InputField InputField;

    public string currentSimId = "0";

    [DllImport("__Internal")]
    private static extern void AddMessageToChat(string username);

    [DllImport("__Internal")]
    private static extern void ConnectToChat();

    [DllImport("__Internal")]
    private static extern void GetCurrentSimId();

    int interval = 0;
    public void ReceiveChatMessage(string message)
    {
        //^DanTheMan joined the party!DanTheMan^Yoyoyo^DanTheMan^Wut wut?^
        MessageWindow.text = "";
        Debug.Log("in Unity, chat message is: " + message);
        string[] chatMessages = message.Split('^');
        interval = chatMessages.Length - 5;
        if (interval < 0)
        {
            for (int i = 0; i < chatMessages.Length - 1; i++)
            {

                string chatName = chatMessages[i];
                i += 1;
                MessageWindow.text += chatName + ":" + chatMessages[i] + "\n";


            }
        }
        else
        {
            for (int i = interval; i < chatMessages.Length - 1; i++)
            {

                string chatName = chatMessages[i];
                i += 1;
                MessageWindow.text += chatName + ":" + chatMessages[i] + "\n";


            }
        }


    }

    public void SendMessageToChat()
    {
        string chatMessage = InputField.text;
        Debug.Log("message to send is: " + chatMessage);
        AddMessageToChat(chatMessage);
    }

    public void StartChatConnection()
    {
        ConnectToChat();
    }

    public void ReturnSimId(string simId)
    {
        currentSimId = simId;
    }
    // Start is called before the first frame update
    void Start()
    {
        GetCurrentSimId();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

