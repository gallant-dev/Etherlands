using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Player_Chat : NetworkBehaviour {

    GameObject chatPanel;
    InputField chatInput;
    [SerializeField]
    EventSystem system;
    NetworkLobbyManager_HG networkManager;
    Player_Menus menus;
    GameObject chatWindowContent;
    GameObject chatScrollView;
    public UserSettings_HG userSettings;

    public bool isChatSelected;
    public bool isChatDisplayed;

    List<Text> chatMessageList = new List<Text>();

	void Start () {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager_HG>();
        chatPanel = GameObject.Find("ChatPanel");
        chatWindowContent = GameObject.Find("ChatWindowContent");
        chatScrollView = GameObject.Find("ChatScrollView");

        chatInput = GameObject.Find("ChatInput").GetComponent<InputField>();

        if (isLocalPlayer)
        {
            menus = GetComponent<Player_Menus>();
            system = EventSystem.current;

            //Deactivate chat window on start
            if (networkManager.playScene == SceneManager.GetActiveScene().name)
            {
                StartCoroutine(ChatWindowCountdown());
            }
        }
	}

    void LateUpdate()
    {
        if (isLocalPlayer && system != null)
        {
            if(system.currentSelectedGameObject == chatInput.gameObject)
            {
                DisableControls();
            }

            if (Input.GetButtonDown("Submit") && chatInput.text != "" && system.currentSelectedGameObject == chatInput.gameObject)
            {
                SendMessageInChat(chatInput.text);
                system.SetSelectedGameObject(null);
                isChatSelected = false;
                EnableControls();
                chatInput.text = "";
            }
            //On enter set the chat input to the current selected object if it isn't already it.
            else if (Input.GetButtonDown("Submit") && (system.currentSelectedGameObject != chatInput.gameObject || system.currentSelectedGameObject != null))
            {
                //Set chat window items active
                chatInput.gameObject.SetActive(true);
                chatPanel.gameObject.SetActive(true);

                //Set chat input to current selected object.
                system.SetSelectedGameObject(chatInput.gameObject, new BaseEventData(system));

                //Disable character controls and make cursor visible.
                //DisableControls();

                //Set isChatSelected true, so that Player_Menus can function.
                isChatSelected = true;

                Color chatPanelColour = chatPanel.GetComponent<Image>().color;
                chatPanelColour.a = userSettings.chatPanelAlpha;
                chatPanel.GetComponent<Image>().color = chatPanelColour;
            }

            //On escape and if chat input is current selected gameobject close chat and reactive controls.
            if (Input.GetButtonDown("Cancel") && system.currentSelectedGameObject == chatInput.gameObject && isChatSelected)
            {
                //Set current selected gameobject to null.
                system.SetSelectedGameObject(null);

                //Reactivate character controls.
                EnableControls();

                //Set isChatSelected false, so that Player_Menus can function.
                isChatSelected = false;
            }
        }
        else if(isLocalPlayer && system == null)
        {
            system = EventSystem.current;
        }
    }

    public void DisableControls()
    {
        if (networkManager.playScene == SceneManager.GetActiveScene().name)
        {
            menus.aimTarget.SetActive(false);
            menus.player.GetComponent<Player_Controller>().isControllable = false;
            menus.cam.GetComponent<Player_Camera>().enabled = false;
            Cursor.visible = true;
        }
    }

    public void EnableControls()
    {
        if (networkManager.playScene == SceneManager.GetActiveScene().name)
        {
            menus.aimTarget.SetActive(true);
            menus.player.GetComponent<Player_Controller>().isControllable = true;
            menus.cam.GetComponent<Player_Camera>().enabled = true;
            Cursor.visible = false;
        }
    }

    public void SendMessageInChat(string message)
    {
        CmdChat(networkManager.localPlayerDisplayName + " : " + message);
    }

    IEnumerator ChatWindowCountdown()
    {
        yield return new WaitForSecondsRealtime(15);

        CanvasRenderer[] renderers = chatPanel.GetComponentsInChildren<CanvasRenderer>();

        foreach(CanvasRenderer renderer in renderers)
        {
            if (renderer.name != "Scroll View")
            {
                renderer.SetAlpha(0f);
            }
        }

        isChatDisplayed = false;
    }

    [Command]
    void CmdChat(string message)
    {
        RpcChatUpdate(message);
    }

    [ClientRpc]
    void RpcChatUpdate(string message)
    {
        if (!isChatDisplayed)
        {
            CanvasRenderer[] renderers = chatPanel.GetComponentsInChildren<CanvasRenderer>();

            foreach (CanvasRenderer renderer in renderers)
            {
                if (renderer.name != "Scroll View")
                {
                    renderer.SetAlpha(1f);
                }
            }
        }

        //Create new gameObject, name it and add a text component.
        GameObject newGO = new GameObject();
        newGO.name = "ChatMessage";
        newGO.AddComponent<Text>();

        //Get text component and set font, colour, parent, and scale according to user settings.
        Text newText = newGO.GetComponent<Text>();
        chatMessageList.Add(newText);

        if (!isLocalPlayer)
        {
            newText.font = userSettings.nonLocalUserChatFont;
            newText.fontStyle = userSettings.nonLocalUserChatFontStyle;
            newText.color = userSettings.nonLocalUserChatColour;
        }
        else
        {
            newText.font = userSettings.localUserChatFont;
            newText.fontStyle = userSettings.localUserChatFontStyle;
            newText.color = userSettings.localUserChatColour;
        }

        newText.verticalOverflow = VerticalWrapMode.Overflow;
        newText.lineSpacing = 0.9f;
        newText.text = message;
        newText.fontSize = userSettings.fontSize;


        chatWindowContent = GameObject.Find("ChatWindowContent");
        float scrollWidth = chatScrollView.GetComponent<RectTransform>().rect.width;
        newText.GetComponent<RectTransform>().SetParent(chatWindowContent.transform);
        Canvas.ForceUpdateCanvases();

        int lineCount;

        if (scrollWidth < 700 && newText.cachedTextGenerator.lineCount > 11)//9 was original;
        {
            lineCount = 3;
        }
        else if(newText.cachedTextGenerator.lineCount > 9 || scrollWidth < 700 && newText.cachedTextGenerator.lineCount > 7)
        {
            lineCount = 2;
        }
        else
        {
            lineCount = 1;
        }


        newText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 15f * lineCount);
        Debug.Log(newText.cachedTextGenerator.lineCount);
        newText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ( scrollWidth-2f));
        newText.transform.localScale = new Vector3(1, 1, 1);

        //Adjust content window size to fit new messages.
        RectTransform chatWindowRect = chatWindowContent.GetComponent<RectTransform>();
        float chatWindowLength = chatWindowRect.rect.height;
        chatWindowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, chatWindowLength += 15f * lineCount);

        //Keep most recent entries in view.
        chatPanel = GameObject.Find("ChatPanel");
        chatPanel.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 0;

        //Set bool to true.
        isChatDisplayed = true;
        if (networkManager.playScene == SceneManager.GetActiveScene().name)
        {
            StartCoroutine(ChatWindowCountdown());
        }
    }
}
