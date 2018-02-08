using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class LobbyButtonBehaviour : MonoBehaviour {

	GameObject lobbyPlayerPrefab;

    NetworkLobbyManager_HG networkManager;

	byte characterSelected = 0;

	void Start(){
		networkManager = GameObject.Find ("NetworkManager").GetComponent<NetworkLobbyManager_HG> ();
	}

	public void CharacterSelect(Button button){
		GameObject[] lobbyPlayerArray = GameObject.FindGameObjectsWithTag ("LobbyPlayer");
		foreach (GameObject prefab in lobbyPlayerArray) {
			if (prefab.GetComponent<NetworkLobbyPlayer_Custom>().isLocalPlayer) {
				// Assign lobby player prefab
				lobbyPlayerPrefab = prefab;
				//Character Selecting
				if (button.name == "GideonButton") {
                    lobbyPlayerPrefab.GetComponent<NetworkLobbyPlayer_Custom>().CmdSendCharacterSelectionToServer(1);
                    characterSelected = 1;
                    networkManager.gamePlayerPrefab = networkManager.spawnPrefabs.Find(chmpn => chmpn.name == "Gideon");
				}
			}
		}
	}

	public void ReadyClick (Button button){
        NetworkLobbyPlayer_Custom lobbyPlayer = lobbyPlayerPrefab.GetComponent<NetworkLobbyPlayer_Custom>();
        if (lobbyPlayer.isReady == false)
        {
            if (characterSelected != 0)
            {
                lobbyPlayer.isReady = true;
                GameObject.Find("QuitMatchButton").GetComponent<Button>().enabled = false;
                lobbyPlayer.SendReadyToBeginMessage();
            }
            else
            {
                return;
            }
        }
        else
        {
            lobbyPlayer.isReady = false;
            button.image.color = button.colors.normalColor;
            lobbyPlayer.SendNotReadyToBeginMessage();
        }
	}

	public void QuitMatch(){
		
		networkManager.QuitMatch ();
	}
}
