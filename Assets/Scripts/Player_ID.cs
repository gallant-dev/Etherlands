using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_ID : NetworkBehaviour {

    [SyncVar] public string playerMatchPosition;
	[SyncVar] private string playerUniqueIdentity;
	private NetworkInstanceId playerNetID;
    private Transform myTransform;

    NetworkLobbyManager_HG networkManager;
    Player_Controller controller;

    // Use this for initialization
    void Awake()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager_HG>();
        myTransform = transform;
    }

    public override void  OnStartLocalPlayer(){
        GetNetIdentity();
		SetIdentity();

        playerMatchPosition = networkManager.localPlayerMatchPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if(myTransform.name == "" || myTransform.name.Contains("(Clone)")){
			SetIdentity ();
		}
   
        if(transform.name != playerUniqueIdentity)
        {
            transform.name = playerUniqueIdentity;
        }
	}

	[Client]
	void GetNetIdentity(){
		//playerNetID = GetComponent<NetworkIdentity>().netId;
		CmdTellServerMyIdentity(MakeUniqueIdentity());
	}
	
	void SetIdentity(){
		if(!isLocalPlayer){
            myTransform.name = playerUniqueIdentity;
        }
        else{
            myTransform.name = MakeUniqueIdentity();
		}
	}

	string MakeUniqueIdentity(){
		string uniqueIdentity = networkManager.localPlayerMatchPosition + " " + networkManager.localPlayerDisplayName + " " + myTransform.name.Replace("(Clone)", "") /*+ " " + playerNetID*/;
		return uniqueIdentity;
	}

	[Command]
	void CmdTellServerMyIdentity(string name){
		playerUniqueIdentity = name;
	}
}
