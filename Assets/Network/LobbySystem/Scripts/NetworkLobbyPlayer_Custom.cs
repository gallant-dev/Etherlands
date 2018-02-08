using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GamePlayer {
	public int playerID;
    public string playerName;
	public int team;
	public int skill;
	public bool isReady;

	public GamePlayer (int newPlayerID, string newName, int newTeam, int newSkill, bool newReady){
		playerID = newPlayerID;
        playerName = newName;
		team = newTeam;
		skill = newSkill;
		isReady = newReady;
	}
}

public class NetworkLobbyPlayer_Custom : NetworkLobbyPlayer {
    
	[SyncVar] public string localPlayerDisplayName;
	public string lastName;
    [SyncVar(hook = "OnParentNameChange")]
    public string parentName;

	[SyncVar] public byte characterSelected = 0;
	public byte lastCharacterSelected = 0;

    string lastMatchPosition;

	[SyncVar] public bool isReady = false;
	[SyncVar] public bool lastReadyStatus = false;

	public GamePlayer player;
    public Outline playerPortraitOutline;

    public NetworkLobbyManager_HG networkManager;

    public bool dontDestroyOnLoad;

    void Awake()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnClientReady(bool readyState)
    {
        //Need to make it so playerList is updated with ReadyStatus'.
        base.OnClientReady(readyState);
    }

    void Start()
    {
            networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager_HG>();
        if (isLocalPlayer) {
            CmdSendNameChangeToServer(networkManager.localPlayerDisplayName);
            CmdRequestParentName();

            if (networkManager.skipMatchmakingLAN)
            {
                SendReadyToBeginMessage();
            }
        }
    }

    void LateUpdate()
    {
            UpdateName();
            UpdateParent();
            AssignPortrait();

        if (isLocalPlayer)
        {
            SetPlayerMatchPosition();
        }
    }

    void UpdateName()
    {
        if(name != localPlayerDisplayName || GetComponentInChildren<Text>().text != localPlayerDisplayName)
        {
            name = localPlayerDisplayName;
            GetComponentInChildren<Text>().text = localPlayerDisplayName;
        }
    }

    void UpdateParent()
    {
        if (parentName != null && GameObject.Find(parentName) != null)
        {
            if (transform.parent == null || transform.parent.name != parentName)
            {
                transform.SetParent(GameObject.Find(parentName).transform);
                transform.localScale = Vector3.one;
                transform.localPosition = Vector3.zero;
                if (transform.parent.localScale == new Vector3(-1, 1, 1))
                {
                    GetComponentInChildren<Text>().transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }



    }

    void SetPlayerMatchPosition()
    {
        if (parentName == "FirstPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "1P";
            lastMatchPosition = "1P";
        }
        else if (parentName == "SecondPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "2P";
            lastMatchPosition = "2P";
        }
        else if (parentName == "ThirdPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "3P";
            lastMatchPosition = "3P";
        }
        else if (parentName == "FourthPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "4P";
            lastMatchPosition = "4P";
        }
        else if (parentName == "FifthPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "5P";
            lastMatchPosition = "5P";
        }
        else if (parentName == "SixthPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "6P";
            lastMatchPosition = "6P";
        }
        else if (parentName == "SeventhPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "7P";
            lastMatchPosition = "7P";
        }
        else if (parentName == "EigthPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "8P";
            lastMatchPosition = "8P";
        }
        else if (parentName == "NinthPlayerBox" && lastMatchPosition != "1P")
        {
            networkManager.localPlayerMatchPosition = "9P";
            lastMatchPosition = "9P";
        }
        else if (parentName == "TenthPlayerBox" && lastMatchPosition != "10P")
        {
            networkManager.localPlayerMatchPosition = "10P";
            lastMatchPosition = "10P";
        }
    }
 
    void AssignPortrait(){
		if (characterSelected == 1 && characterSelected != lastCharacterSelected) {
			Image[] prefabChildren = GetComponentsInChildren<Image> ();
			foreach(Image child in prefabChildren){
				if(child.name == "PlayerPortrait"){
					child.GetComponent<Image>().sprite = GameObject.Find("GideonButton").GetComponent<Button>().image.sprite;
					//						child.GetComponent<Image> ().color.a = 255;
				}
			}
		}
		lastCharacterSelected = characterSelected;
	}

	public void SendSelectionToNetworkManager(){
		GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager_HG>().characterLockedIn = characterSelected;
	}


    [Command]
    public void CmdSendNameChangeToServer(string name)
    {
        localPlayerDisplayName = name;
        gameObject.name = name;
    }

    [Command]
    public void CmdRequestParentName()
    {
        parentName = networkManager.GetParentName(slot);
    }

    [Command]
    public void CmdSendCharacterSelectionToServer(byte chrSlctn)
    {
        characterSelected = chrSlctn;
    }

    [ClientRpc]
    public void RpcScreenFade(bool isPositive)
    {
        if (isPositive)
        {
            networkManager.screenFadePositive = true;
        }
        else
        {
            networkManager.screenFadePositive = false;
        }
    }
    public void OnParentNameChange(string name)
    {
        parentName = name;
    }
    
}

