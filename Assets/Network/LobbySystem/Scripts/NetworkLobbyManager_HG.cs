using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;


public class NetworkLobbyManager_HG : NetworkLobbyManager
{
    public Text matchMakingStepText;

    public int tryCount = 1;

    public string localPlayerDisplayName;
    public int localPlayerConnectionID;
    public int localPlayerSkill = 30;
    public string localPlayerMatchPosition;
    public GamePlayer localGamePlayer;

    public int matchMaxPlayers;
    public int matchSkillMax = 150;

    public int playersOnBlueTeam = 0;
    public int blueTeamSkill = 0;
    public int playersOnRedTeam = 0;
    public int redTeamSkill = 0;

    List<GamePlayer> playerList = new List<GamePlayer>();
    public List<GamePlayer> bluePlayerList = new List<GamePlayer>();
    public List<GamePlayer> redPlayerList = new List<GamePlayer>();

    public byte characterLockedIn;

    public GameObject lobbyGUI;

    public CanvasRenderer screenFade;
    public bool screenFadePositive = true;
    public float screenFadeRate = 1f;

    //Win Condition References.
    public CanvasRenderer winConditionCanvas;
    public float winConditionValue = 1.0f;

    public bool skipMatchmakingLAN = false;

    void Start()
    {
        screenFadePositive = false;
        SceneManager.activeSceneChanged += SetScreenFadePositiveFalse;
    }

    void SetScreenFadePositiveFalse(Scene scene, Scene nextScene)
    {
        Debug.Log("SetScreenFadePositiveFalse");
        if (skipMatchmakingLAN && nextScene.name == lobbyScene)
        {
            return;
        }
        else
        {
            screenFadePositive = false;
        }
    }

    void LateUpdate()
    {
        ScreenFade();

        if (SceneManager.GetActiveScene().name == lobbyScene)
        {
            lobbyGUI.SetActive(true);
            if (winConditionCanvas.GetAlpha() != 0f)
            {
                winConditionCanvas.SetAlpha(0f);
            }
        }
    }

    void ScreenFade()
    {
        if (screenFadePositive && screenFade.GetAlpha() != 1f)
        {
            screenFade.SetAlpha(1f);
        }
        else if (!screenFadePositive && screenFade.GetAlpha() != 0f)
        {
            screenFade.SetAlpha(Mathf.Lerp(screenFade.GetAlpha() * screenFade.GetAlpha(), 0f, screenFadeRate * Time.deltaTime));
        }
    }

    public void MatchMaker(int matchType)
    {
        StartMatchMaker();
        if (matchMakingStepText.isActiveAndEnabled)
        {
            matchMakingStepText.text = "Network Status: " + System.Environment.NewLine + "Started MatchMaker!";
        }
        matchMaker.ListMatches(0, 10, "Etherlands", true, 0, 1, OnMatchList);
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {


        if (success)
        {
            if (matchList.Count == 0)
            {

                Debug.Log("No Matches Listed!");

                if (tryCount <= 3)
                {
                    matchMaker.ListMatches(0, 10, "Etherlands", true, 0, 1, OnMatchList);
                    if (matchMakingStepText.isActiveAndEnabled)
                    {
                        matchMakingStepText.text = "No Matches Listed!" + System.Environment.NewLine + "Attempt " + tryCount;
                    }
                    Debug.Log("Attempt" + tryCount);

                    ++tryCount;
                }
                else
                {
                    if (matchMakingStepText.isActiveAndEnabled)
                    {
                        matchMakingStepText.text = "No Matches Listed!" + System.Environment.NewLine + "Attempting to Create Match!";
                    }
                    matchMaker.CreateMatch("Etherlands", 10, true, "", "", "", 0, 1, OnMatchCreate);
                }
            }
            else
            {
                Debug.Log("Matches Listed!");
                if (matchMakingStepText.isActiveAndEnabled)
                {
                    matchMakingStepText.text = "Matches Listed!" + System.Environment.NewLine + "Attempting to join a match!";
                }
                matchMaker.JoinMatch(matchList[0].networkId, "", "", "", 0, 1, OnMatchJoined);

            }
        }
        else
        {
            Debug.Log(extendedInfo);
        }
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            screenFadePositive = true;
            StartHost(matchInfo);
            SendReturnToLobby();
            matchMakingStepText.text = "Match Created!";
            Debug.Log("Created Match!");
        }
        else
        {
            if (matchMakingStepText.isActiveAndEnabled)
            {
                matchMakingStepText.text = "Failed Creating Match!";
            }
            Debug.Log("Create Match Failed!" + " " + extendedInfo);
        }

    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            screenFadePositive = true;

            StartClient(matchInfo);

            if (matchMakingStepText.isActiveAndEnabled)
            {
                matchMakingStepText.text = "Joined Match!";
            }
            Debug.Log("Match Joined!");
        }
        else
        {
            if (matchMakingStepText.isActiveAndEnabled)
            {
                matchMakingStepText.text = "Failed Joining Match!";
            }

            Debug.Log("Match Join Failed!" + " " + extendedInfo);
        }
    }

    //Players need to be added after scene change or lobbyplayer object will not survive scene change.
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (!conn.isReady || networkSceneName == playScene)
        {
            Debug.Log("playScene");
            base.OnClientSceneChanged(conn);
        }

        //Host player added.
        if (networkSceneName == lobbyScene || SceneManager.GetActiveScene().name == lobbyScene)
        {
            Debug.Log("lobbyScene");
            lobbyGUI.SetActive(true);
            ClientScene.AddPlayer(conn, 0);
        }
        else if(networkSceneName != lobbyScene || SceneManager.GetActiveScene().name != lobbyScene)
        {
            lobbyGUI.SetActive(false);
        }
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        base.OnLobbyClientConnect(conn);
    }

    public override void OnLobbyServerSceneChanged(string sceneName)
    {
        Debug.Log("LobbyServerSceneChanged!");
        base.OnLobbyServerSceneChanged(sceneName);
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("OnLobbyServerCreateGamePlayer");
        return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Debug.Log("OnLobbyServerSceneLoadedForPlayer");
       
        NetworkLobbyPlayer_Custom lobbyPlayerInfo = lobbyPlayer.GetComponent<NetworkLobbyPlayer_Custom>();
        Vector3 spawnPoint;

        if (playerList.Find(player => player.playerID == lobbyPlayerInfo.connectionToClient.connectionId).team == 0)
        {
            spawnPoint = GameObject.Find("BlueSpawnPoint").transform.position;
        }
        else
        {
            spawnPoint = GameObject.Find("RedSpawnPoint").transform.position;
        }

        gamePlayer.GetComponent<Player_Controller>().localGamePlayer = playerList.Find(player => player.playerID == lobbyPlayerInfo.connectionToClient.connectionId);
        gamePlayer.transform.position = spawnPoint;
        gamePlayer.GetComponent<Player_Stats>().respawnPoint = spawnPoint;

        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("LobbyClientSceneChanged!");
        base.OnLobbyClientSceneChanged(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        Debug.Log("PlayerAdded!");

        if (SceneManager.GetActiveScene().name == lobbyScene)
        {
            AddPlayerToList(conn);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Client Connected!");
        if (SceneManager.GetActiveScene().name != lobbyScene)
        {
            SceneManager.LoadScene(lobbyScene);

            //Client players added.
            if (!conn.isReady)
            {
                ClientScene.Ready(conn);
                ClientScene.AddPlayer(conn, 0);
            }
        }

        //base.OnClientConnect(conn);
    }

    public override void OnLobbyClientAddPlayerFailed()
    {
        Debug.Log("Attempt to add player failed.");
        base.OnLobbyClientAddPlayerFailed();
    }

    public override void OnLobbyStartServer()
    {
        Debug.Log("Start Server!");
        base.OnLobbyStartServer();
    }

    public override void OnLobbyClientEnter()
    {
        Debug.Log("Client Entered!");
        base.OnLobbyClientEnter();

    }

    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        Debug.Log("Player Connected To Host" + " Connection ID: " + conn.connectionId);
        base.OnLobbyServerConnect(conn);
    }

    public override void OnLobbyStartHost()
    {
        base.OnLobbyStartHost();
        Debug.Log("OnLobbyStartHost");
    }

    public override void OnLobbyStartClient(NetworkClient lobbyClient)
    {
        base.OnLobbyStartClient(lobbyClient);
        Debug.Log("OnLobbyStartClient");
    }

    public override void OnStartClient(NetworkClient lobbyClient)
    {
        base.OnStartClient(lobbyClient);
    }

    public override void OnLobbyServerPlayersReady()
    {
        screenFadePositive = true;

        NetworkLobbyPlayer_Custom[] lobbyPlayers = NetworkLobbyPlayer_Custom.FindObjectsOfType<NetworkLobbyPlayer_Custom>();

        foreach(NetworkLobbyPlayer_Custom player in lobbyPlayers)
        {
            player.RpcScreenFade(true);
        }

        base.OnLobbyServerPlayersReady();
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        base.OnLobbyServerDisconnect(conn);
        Debug.Log("OnLobbyServerDisconnect");
        GamePlayer removedPlayer = playerList.Find(playerR => playerR.playerID == conn.connectionId);

        if (removedPlayer.team == 0)
        {
            blueTeamSkill -= removedPlayer.skill;
            bluePlayerList.Remove(removedPlayer);
        }
        else
        {
            redTeamSkill -= removedPlayer.skill;
            redPlayerList.Remove(removedPlayer);
        }

        playerList.Remove(removedPlayer);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        Debug.Log("OnServerRemovePlayer");
    }

    //Called OnServerAddPlayer
    public void AddPlayerToList(NetworkConnection conn)
    {
        GamePlayer newPlayer = AssignPlayerToTeam(conn);
        playerList.Add(newPlayer);

        if (newPlayer.team == 0)
        {
            blueTeamSkill += newPlayer.skill;
        }
        else
        {
            redTeamSkill += newPlayer.skill;
        }
    }

    GamePlayer AssignPlayerToTeam(NetworkConnection conn)
    {
        if (playersOnBlueTeam <= playersOnRedTeam && blueTeamSkill <= redTeamSkill)
        {
            return new GamePlayer(conn.connectionId, "", 0, 30, false);
        }
        else
        {
            return new GamePlayer(conn.connectionId, "", 1, 30, false);
        }
    }

    public string GetParentName(byte slot)
    {
        foreach (GamePlayer playa in playerList)
        {
            if (playa.team == 0 && !bluePlayerList.Contains(playa))
            {
                bluePlayerList.Add(playa);
            }
            else if(playa.team == 1 && !redPlayerList.Contains(playa))
            {
                redPlayerList.Add(playa);
            }
        }

        if (lobbySlots[slot] != null && bluePlayerList.Count == 1 && lobbySlots[slot].connectionToClient.connectionId == bluePlayerList[0].playerID)
        {
            return "FirstPlayerBox";
        }
        else if (lobbySlots[slot] != null && bluePlayerList.Count == 2 && lobbySlots[slot].connectionToClient.connectionId == bluePlayerList[1].playerID)
        {
            return "SecondPlayerBox";
        }
        else if (lobbySlots[slot] != null && bluePlayerList.Count == 3 && lobbySlots[slot].connectionToClient.connectionId == bluePlayerList[2].playerID)
        {
            return "ThirdPlayerBox";
        }
        else if (lobbySlots[slot] != null && bluePlayerList.Count == 4 && lobbySlots[slot].connectionToClient.connectionId == bluePlayerList[3].playerID)
        {
            return "FourthPlayerBox";
        }
        else if (lobbySlots[slot] != null && bluePlayerList.Count == 5 && lobbySlots[slot].connectionToClient.connectionId == bluePlayerList[4].playerID)
        {
            return "FifthPlayerBox";
        }
        else if (lobbySlots[slot] != null && redPlayerList.Count == 1 && lobbySlots[slot].connectionToClient.connectionId == redPlayerList[0].playerID)
        {
            return "SixthPlayerBox";
        }
        else if (lobbySlots[slot] != null && redPlayerList.Count == 2 && lobbySlots[slot].connectionToClient.connectionId == redPlayerList[1].playerID)
        {
            return "SeventhPlayerBox";
        }
        else if (lobbySlots[slot] != null && redPlayerList.Count == 3 && lobbySlots[slot].connectionToClient.connectionId == redPlayerList[2].playerID)
        {
            return "EighthPlayerBox";
        }
        else if (lobbySlots[slot] != null && redPlayerList.Count == 4 && lobbySlots[slot].connectionToClient.connectionId == redPlayerList[3].playerID)
        {
            return "NinthPlayerBox";
        }
        else if (lobbySlots[slot] != null && redPlayerList.Count == 4 && lobbySlots[slot].connectionToClient.connectionId == redPlayerList[4].playerID)
        {
            return "TenthPlayerBox";
        }
        else
        {
            Debug.Log("No Position Determined");
            return null;
        }
    }

    public void SkipLobbyLAN()
    {
        screenFadePositive = true;
        skipMatchmakingLAN = true;
        StartHost();
        ServerChangeScene(playScene);
    }

    public void QuitMatch()
    {
        screenFadePositive = true;
        SceneManager.LoadScene(0);
        StopClient();
        StopMatchMaker();
    }
}
