using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Object_SyncLandingPos : NetworkBehaviour
{
    //Win Condition References
    public float winConditionValue = 1f;
    NetworkLobbyManager_HG networkManager;
    public bool isMatchOver;

    [SyncVar(hook = "OnPositionSyncPosBluNo")]
    float syncLandingPosBlueNorth = 32f;

    [SyncVar(hook = "OnPositionSyncPosBluWe")]
    float syncLandingPosBlueWest = 32f;

    [SyncVar(hook = "OnPositionSyncPosBluSo")]
    float syncLandingPosBlueSouth = 32f;

    [SyncVar (hook = "OnPositionSyncPosRedNo")]
    float syncLandingPosRedNorth = 32f;

    [SyncVar(hook = "OnPositionSyncPosRedEa")]
    float syncLandingPosRedEast = 32f;

    [SyncVar(hook = "OnPositionSyncPosRedSo")]
    float syncLandingPosRedSouth = 32f;


    [SerializeField]
    Transform landingTransformBlueNorth;

    [SerializeField]
    Transform landingTransformBlueWest;

    [SerializeField]
    Transform landingTransformBlueSouth;

    [SerializeField]
    Transform landingTransformRedNorth;

    [SerializeField]
    Transform landingTransformRedEast;

    [SerializeField]
    Transform landingTransformRedSouth;

    [SerializeField]
    Transform etherTransform;

    Vector3 lastLandingPosBlueNorth;
    Vector3 lastLandingPosBlueWest;
    Vector3 lastLandingPosBlueSouth;
    Vector3 lastLandingPosRedNorth;
    Vector3 lastLandingPosRedEast;
    Vector3 lastLandingPosRedSouth;

    public float landingLevel = 1.2f;
    float closeEnough = 0.05f;
    float lowestPosition = 32f;

    public Transform[] destructiblePathArray;

    // Use this for initialization
    void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager_HG>();
        winConditionValue = networkManager.winConditionValue;
    }


    void LateUpdate()
    {
        //
        if (isServer)
        {
            if (landingTransformBlueNorth.position.y < syncLandingPosBlueNorth)
            {
                syncLandingPosBlueNorth = landingTransformBlueNorth.position.y;
            }
            if (landingTransformBlueWest.position.y < syncLandingPosBlueNorth)
            {
                syncLandingPosBlueWest = landingTransformBlueWest.position.y;
            }
            if (landingTransformBlueSouth.position.y < syncLandingPosBlueNorth)
            {
                syncLandingPosBlueSouth = landingTransformBlueSouth.position.y;
            }
            if (landingTransformRedNorth.position.y < syncLandingPosBlueNorth)
            {
                syncLandingPosRedNorth = landingTransformRedNorth.position.y;
            }
            if (landingTransformRedEast.position.y < syncLandingPosBlueNorth)
            {
                syncLandingPosRedEast = landingTransformRedEast.position.y;
            }
            if (landingTransformRedSouth.position.y < syncLandingPosBlueNorth)
            {
                syncLandingPosRedSouth = landingTransformRedSouth.position.y;
            }
        }

        //Win condition
        if (syncLandingPosBlueWest < winConditionValue && !isMatchOver)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player.GetComponent<Player_Controller>().localGamePlayerBool == true)
                {
                    player.GetComponentInChildren<Player_Controller>().isControllable = false;
                    Player_ID playerID = player.GetComponent<Player_ID>();   
                    if(playerID.playerMatchPosition == "1P"){
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    } 
                    else if(playerID.playerMatchPosition == "2P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                    else if (playerID.playerMatchPosition == "3P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                    else if (playerID.playerMatchPosition == "4P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                    else if (playerID.playerMatchPosition == "5P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                    else if (playerID.playerMatchPosition == "6P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "7P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "8P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "9P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "10P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                }
            }

            networkManager.winConditionCanvas.SetAlpha(1f);
            isMatchOver = true;
        }
        else if (syncLandingPosRedEast < winConditionValue && !isMatchOver)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player.GetComponent<Player_Controller>().localGamePlayerBool == true)
                {
                    player.GetComponentInChildren<Player_Controller>().isControllable = false;
                    Player_ID playerID = player.GetComponent<Player_ID>();
                    if (playerID.playerMatchPosition == "1P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "2P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "3P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "4P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "5P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "VICTORY!";
                    }
                    else if (playerID.playerMatchPosition == "6P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                    else if (playerID.playerMatchPosition == "7P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                    else if (playerID.playerMatchPosition == "8P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                    else if (playerID.playerMatchPosition == "9P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                    else if (playerID.playerMatchPosition == "10P")
                    {
                        networkManager.winConditionCanvas.GetComponentInChildren<Text>().text = "DEFEAT!";
                    }
                }
            }

            networkManager.winConditionCanvas.SetAlpha(1f);
            isMatchOver = true;
        }
        else if (syncLandingPosRedEast > winConditionValue && syncLandingPosBlueWest > winConditionValue && !isMatchOver)
        {

            networkManager.winConditionCanvas.SetAlpha(0f);
        }
    }

    void OnPositionSyncPosBluNo(float landPos)
    {
        syncLandingPosBlueNorth = landPos;
        if (landPos < (lowestPosition - landingLevel))
        {
            etherTransform.position = new Vector3(etherTransform.position.x, etherTransform.position.y - 2f, etherTransform.position.z);
            lowestPosition = landPos;
            LowerDestructiblePaths();

        }

        Vector3 blueNor = new Vector3(landingTransformBlueNorth.position.x, syncLandingPosBlueNorth, landingTransformBlueNorth.position.z);
        if (Vector3.Distance(landingTransformBlueNorth.position, lastLandingPosBlueNorth) > closeEnough)
        {
            lastLandingPosBlueNorth = landingTransformBlueNorth.position;
            landingTransformBlueNorth.position = blueNor;
        }
    }
    void OnPositionSyncPosBluWe(float landPos)
    {
        syncLandingPosBlueWest = landPos;
        if (landPos < (lowestPosition - landingLevel))
        {
            etherTransform.position = new Vector3(etherTransform.position.x, etherTransform.position.y - 2f, etherTransform.position.z);
            lowestPosition = landPos;
            LowerDestructiblePaths();
        }

        Vector3 blueWes = new Vector3(landingTransformBlueWest.position.x, syncLandingPosBlueWest, landingTransformBlueWest.position.z);
        if (Vector3.Distance(landingTransformBlueWest.position, lastLandingPosBlueWest) > closeEnough)
        {
            lastLandingPosBlueWest = landingTransformBlueWest.position;
            landingTransformBlueWest.position = blueWes;
        }
    }
    void OnPositionSyncPosBluSo(float landPos)
    {
        syncLandingPosBlueSouth = landPos;
        if (landPos < (lowestPosition - landingLevel))
        {
            etherTransform.position = new Vector3(etherTransform.position.x, etherTransform.position.y - 2f, etherTransform.position.z);
            lowestPosition = landPos;
            LowerDestructiblePaths();
        }

        Vector3 blueSou = new Vector3(landingTransformBlueSouth.position.x, syncLandingPosBlueSouth, landingTransformBlueSouth.position.z);
        if (Vector3.Distance(landingTransformBlueSouth.position, lastLandingPosBlueSouth) > closeEnough)
        {
            lastLandingPosBlueSouth = landingTransformBlueSouth.position;
            landingTransformBlueSouth.position = blueSou;
        }
    }
    void OnPositionSyncPosRedNo(float landPos)
    {
        syncLandingPosRedNorth = landPos;
        if (landPos < (lowestPosition - landingLevel))
        {
            etherTransform.position = new Vector3(etherTransform.position.x, etherTransform.position.y - 2f, etherTransform.position.z);
            lowestPosition = landPos;
            LowerDestructiblePaths();
        }

        Vector3 RedNor = new Vector3(landingTransformRedNorth.position.x, syncLandingPosRedNorth, landingTransformRedNorth.position.z);
        if (Vector3.Distance(landingTransformRedNorth.position, lastLandingPosRedNorth) > closeEnough)
        {
            lastLandingPosRedNorth = landingTransformRedNorth.position;
            landingTransformRedNorth.position = RedNor;
        }
    }
    void OnPositionSyncPosRedEa(float landPos)
    {
        syncLandingPosRedEast = landPos;
        if (landPos < (lowestPosition - landingLevel))
        {
            etherTransform.position = new Vector3(etherTransform.position.x, etherTransform.position.y - 2f, etherTransform.position.z);
            lowestPosition = landPos;
            LowerDestructiblePaths();
        }

        Vector3 RedEas = new Vector3(landingTransformRedEast.position.x, syncLandingPosRedEast, landingTransformRedEast.position.z);
        if (Vector3.Distance(landingTransformRedEast.position, lastLandingPosRedEast) > closeEnough)
        {
            lastLandingPosRedEast = landingTransformRedEast.position;
            landingTransformRedEast.position = RedEas;
        }
    }
    void OnPositionSyncPosRedSo(float landPos)
    {
        syncLandingPosRedSouth = landPos;
        if(landPos < (lowestPosition - landingLevel))
        {
            etherTransform.position = new Vector3(etherTransform.position.x, etherTransform.position.y - 2f, etherTransform.position.z);
            lowestPosition = landPos;
            LowerDestructiblePaths();
        }

        Vector3 RedSou = new Vector3(landingTransformRedSouth.position.x, syncLandingPosRedSouth, landingTransformRedSouth.position.z);
        landingTransformRedSouth.position = RedSou;
        if (Vector3.Distance(landingTransformRedSouth.position, lastLandingPosRedSouth) > closeEnough)
        {
            lastLandingPosRedSouth = landingTransformRedSouth.position;
            landingTransformRedSouth.position = RedSou;
        }
    }

    void LowerDestructiblePaths()
    {
        foreach(Transform path in destructiblePathArray)
        {
            path.position = new Vector3(path.position.x, path.position.y - 2f, path.position.z);
        }
    }
}