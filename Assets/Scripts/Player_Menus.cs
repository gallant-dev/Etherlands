using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player_Menus : NetworkBehaviour {

	public GameObject player;
	public Camera cam;

	private GameObject title;
	private GameObject menu;
    private GameObject settingsMenu;
    private GameObject statMenu;
	private GameObject playerBuildDisplay;
    private GameObject[] menuArray;

    public Image leftTalentTextBox;
	public Text leftTalentText;
	public Image leftTalentMouseDiagram;
	public Image leftTalentLeftMouseButton;
	public Image leftTalentRightMouseButton;

	public Text rightStatText;
	public Text rightStatValueText;
    private GameObject lowerUI;
    private GameObject respawnCountdown;


	public Text healthText;
	public Text energyText;
	public Text chargeText;

	public Text damageFirstTalentText;
	public Text damageSecondTalentText;
	public Text damageThirdTalentText;
	public Text defenseFirstTalentText;
	public Text defenseSecondTalentText;
	public Text defenseThirdTalentText;
	public Text supportFirstTalentText;
	public Text supportSecondTalentText;
	public Text supportThirdTalentText;
	public Text mobilityFirstTalentText;
	public Text mobilitySecondTalentText;
	public Text mobilityThirdTalentText;


    //Stat Text References
    public Text healthRegenStatText;
    public Text healthRegenStatTextValue;
    public Text energyRegenStatText;
    public Text energyRegenStatTextValue;
    public Text ArmourStatText;
    public Text ArmourStatTextValue;
    public Text RunSpeedStatText;
    public Text RunSpeedStatTextValue;
    public Text jumpHeightStatText;
    public Text jumpHeightStatTextValue;
    public Text primaryAbilityDamageStatText;
    public Text primaryAbilityDamageStatTextValue;
    public Text primaryAbilitySpeedStatText;
    public Text primaryAbilitySpeedTextValue;
    public Text primaryAbilityRangeStatText;
    public Text primaryAbilityRangeStatTextValue;
    public Text secondaryAbilityDamageStatText;
    public Text secondaryAbilityDamageStatTextValue;
    public Text secondaryAbilitySpeedStatText;
    public Text secondaryAbilitySpeedStatTextValue;
    public Text secondaryAbilityRangeStatText;
    public Text secondaryAbilityRangeStatTextValue;
    public Text tertiaryAbilityDamageStatText;
    public Text tertiaryAbilityDamageStatTextValue;
    public Text tertiaryAbilitySpeedStatText;
    public Text tertiaryAbilitySpeedStatTextValue;
    public Text tertiaryAbilityRangeStatText;
    public Text tertiaryAbilityRangeStatTextValue;

    public GameObject aimTarget;
	public Rigidbody playerRB;

	public bool menuBool = false;

    Player_Chat playerChat;

	void Start (){
		if (isLocalPlayer) {
            
			//Stat Text References
			healthText = GameObject.Find ("HealthText").GetComponent<Text>();	
			energyText = GameObject.Find ("EnergyText").GetComponent<Text>();
			chargeText = GameObject.Find ("LimiterChargeText").GetComponent<Text>();	

			//Talent Text References
			damageFirstTalentText = GameObject.Find ("DamageFirstTalentText").GetComponent<Text> ();
			damageSecondTalentText = GameObject.Find ("DamageSecondTalentText").GetComponent<Text> ();
			damageThirdTalentText = GameObject.Find ("DamageThirdTalentText").GetComponent<Text> ();
			defenseFirstTalentText = GameObject.Find ("DefenseFirstTalentText").GetComponent<Text> ();
			defenseSecondTalentText = GameObject.Find ("DefenseSecondTalentText").GetComponent<Text> ();
			defenseThirdTalentText = GameObject.Find ("DefenseThirdTalentText").GetComponent<Text> ();
			supportFirstTalentText = GameObject.Find ("SupportFirstTalentText").GetComponent<Text> ();
			supportSecondTalentText = GameObject.Find ("SupportSecondTalentText").GetComponent<Text> ();
			supportThirdTalentText = GameObject.Find ("SupportThirdTalentText").GetComponent<Text> ();
			mobilityFirstTalentText = GameObject.Find ("MobilityFirstTalentText").GetComponent<Text> ();
			mobilitySecondTalentText = GameObject.Find ("MobilitySecondTalentText").GetComponent<Text> ();
			mobilityThirdTalentText = GameObject.Find ("MobilityThirdTalentText").GetComponent<Text> ();

            //Stat Text References
            healthRegenStatText = GameObject.Find("HealthRegenStatText").GetComponent<Text>();
            healthRegenStatTextValue = GameObject.Find("HealthRegenStatTextValue").GetComponent<Text>();
            energyRegenStatText = GameObject.Find("EnergyRegenStatText").GetComponent<Text>();
            energyRegenStatTextValue = GameObject.Find("EnergyRegenStatTextValue").GetComponent<Text>();
            ArmourStatText = GameObject.Find("ArmourStatText").GetComponent<Text>();
            ArmourStatTextValue = GameObject.Find("ArmourStatTextValue").GetComponent<Text>();
            RunSpeedStatText = GameObject.Find("RunSpeedStatText").GetComponent<Text>();
            RunSpeedStatTextValue = GameObject.Find("RunSpeedStatTextValue").GetComponent<Text>();
            jumpHeightStatText = GameObject.Find("JumpHeightStatText").GetComponent<Text>();
            jumpHeightStatTextValue = GameObject.Find("JumpHeightStatTextValue").GetComponent<Text>();
            primaryAbilityDamageStatText = GameObject.Find("PrimaryAbilityDamageStatText").GetComponent<Text>();
            primaryAbilityDamageStatTextValue = GameObject.Find("PrimaryAbilityDamageStatTextValue").GetComponent<Text>();
            primaryAbilitySpeedStatText = GameObject.Find("PrimaryAbilitySpeedStatText").GetComponent<Text>();
            primaryAbilitySpeedTextValue = GameObject.Find("PrimaryAbilitySpeedStatTextValue").GetComponent<Text>();
            primaryAbilityRangeStatText = GameObject.Find("PrimaryAbilityRangeStatText").GetComponent<Text>();
            primaryAbilityRangeStatTextValue = GameObject.Find("PrimaryAbilityRangeStatTextValue").GetComponent<Text>();
            secondaryAbilityDamageStatText = GameObject.Find("SecondaryAbilityDamageStatText").GetComponent<Text>();
            secondaryAbilityDamageStatTextValue = GameObject.Find("SecondaryAbilityDamageStatTextValue").GetComponent<Text>();
            secondaryAbilitySpeedStatText = GameObject.Find("SecondaryAbilitySpeedStatText").GetComponent<Text>();
            secondaryAbilitySpeedStatTextValue = GameObject.Find("SecondaryAbilitySpeedStatTextValue").GetComponent<Text>();
            secondaryAbilityRangeStatText = GameObject.Find("SecondaryAbilityRangeStatText").GetComponent<Text>();
            secondaryAbilityRangeStatTextValue = GameObject.Find("SecondaryAbilityRangeStatTextValue").GetComponent<Text>();
            tertiaryAbilityDamageStatText = GameObject.Find("TertiaryAbilityDamageStatText").GetComponent<Text>();
            tertiaryAbilityDamageStatTextValue = GameObject.Find("TertiaryAbilityDamageStatTextValue").GetComponent<Text>();
            tertiaryAbilitySpeedStatText = GameObject.Find("TertiaryAbilitySpeedStatText").GetComponent<Text>();
            tertiaryAbilitySpeedStatTextValue = GameObject.Find("TertiaryAbilitySpeedStatTextValue").GetComponent<Text>();
            tertiaryAbilityRangeStatText = GameObject.Find("TertiaryAbilityRangeStatText").GetComponent<Text>();
            tertiaryAbilityRangeStatTextValue = GameObject.Find("TertiaryAbilityRangeStatTextValue").GetComponent<Text>();

            title = GameObject.Find ("Title");
			menu = GameObject.Find ("OptionsMenu");
            statMenu = GameObject.Find ("StatMenu");
            settingsMenu = GameObject.Find("SettingsMenu");
            leftTalentTextBox = GameObject.Find ("LeftTalentTextBox").GetComponent<Image>();
			playerBuildDisplay = GameObject.Find ("PlayerBuildDisplay");
            menuArray = new GameObject[] { menu, settingsMenu, statMenu, playerBuildDisplay };

			leftTalentText = leftTalentTextBox.GetComponentInChildren<Text> ();
			leftTalentMouseDiagram = GameObject.Find ("MouseDiagram").GetComponent<Image> ();
			leftTalentLeftMouseButton = GameObject.Find ("LeftMouseButton").GetComponent<Image> ();
			leftTalentRightMouseButton = GameObject.Find ("RightMouseButton").GetComponent<Image> ();
			lowerUI = GameObject.Find ("Lower UI");
            respawnCountdown = GameObject.Find("LocalRespawnCountdownText");

            GameObject.Find("Match Menus").GetComponent<ButtonBehaviour>().GetPlayerObject(gameObject);

            playerChat = GetComponent<Player_Chat>();

            leftTalentTextBox.GetComponent<Image> ().enabled = false;
			leftTalentText.enabled = false;
			leftTalentMouseDiagram.enabled = false;
			leftTalentLeftMouseButton.enabled = false;
			leftTalentRightMouseButton.enabled = false;
			title.SetActive (false);
			menu.SetActive (false);
			statMenu.SetActive (false);
			playerBuildDisplay.SetActive (false);
		}
	
		if (!isLocalPlayer && !cam.isActiveAndEnabled) {
			Camera[] nonLocalCams = Camera.allCameras;
			foreach (Camera cmr in nonLocalCams) {
				if (cmr.isActiveAndEnabled && !cmr.CompareTag("MainCamera")) {
					cam = cmr;
                                     
				}
			}
		}
	}


	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
            OpenOptionsMenu();
            StatsMenu();
            ShowPlayerBuildDisplay();
        }
	}

    void LateUpdate()
    {
        if (playerBuildDisplay != null)
        {
            if (isLocalPlayer && playerBuildDisplay.activeInHierarchy)
            {
                RefreshPlayerBuilds();
            }
            else
            {
                return;
            }
        }
    }

	public void StatsMenu(){
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Tab) && !playerChat.isChatSelected){
			if(menuBool == false){
                CloseCurrentMenus();
				//toggle ui elements
				statMenu.SetActive (true);
				//toggle player components
				player.GetComponentInChildren<Player_Controller>().isControllable/*enabled*/ = false;
				aimTarget.SetActive (false);
				cam.GetComponent<Player_Camera> ().isControllable = false;
				Cursor.visible = true;
				menuBool = true;
			}else{
				//toggle ui elements
				statMenu.SetActive (false);
				//toggle player compontents
				aimTarget.SetActive(true);
				player.GetComponentInChildren<Player_Controller>().isControllable/*enabled*/ = true;
				cam.GetComponent<Player_Camera>().isControllable = true;
				Cursor.visible = false;
				leftTalentText.enabled = false;
				leftTalentTextBox.GetComponent<Image> ().enabled = false;
				leftTalentMouseDiagram.enabled = false;
				leftTalentLeftMouseButton.enabled = false;
				leftTalentRightMouseButton.enabled = false;
				menuBool = false;
			}
		}
	}

    public void CloseCurrentMenus()
    {
        foreach (GameObject menu in menuArray)
        {
            menu.SetActive(false);
        }
    }

	void ShowPlayerBuildDisplay(){
		if(isLocalPlayer && Input.GetKeyDown(KeyCode.BackQuote)){
			if(menuBool == false){
                CloseCurrentMenus();
                //toggle ui elements
                playerBuildDisplay.SetActive (true);
                //toggle player components
                PlayerBuildMenuRemovePlayerBuildUIs();
                respawnCountdown.SetActive(false);
                player.GetComponentInChildren<Player_Controller>().isControllable = false;
				aimTarget.SetActive (false);
				cam.GetComponent<Player_Camera> ().isControllable = false;
				Cursor.visible = true;
                RefreshPlayerBuilds();
                menuBool = true;
			}else{
				//toggle ui elements
				playerBuildDisplay.SetActive (false);
                //toggle player compontents
                respawnCountdown.SetActive(true);
                aimTarget.SetActive(true);
				player.GetComponentInChildren<Player_Controller>().isControllable = true;
				cam.GetComponent<Player_Camera>().isControllable = true;
				Cursor.visible = false;
				leftTalentText.enabled = false;
				leftTalentTextBox.GetComponent<Image> ().enabled = false;
				leftTalentMouseDiagram.enabled = false;
				leftTalentLeftMouseButton.enabled = false;
				leftTalentRightMouseButton.enabled = false;
				menuBool = false;
			}
		}
	}

	void OpenOptionsMenu(){
		if (isLocalPlayer && Input.GetButtonDown("Cancel")) {
			if(menuBool == false){
                CloseCurrentMenus();
                title.SetActive (true);
				menu.SetActive (true);
				aimTarget.SetActive (false);
                menu.GetComponentInParent<ButtonBehaviour>().settingsButton.SetActive(true);
                menu.GetComponentInParent<ButtonBehaviour>().quitButton.SetActive(true);
                player.GetComponentInChildren<Player_Controller>().isControllable = false;
				cam.GetComponent<Player_Camera>().isControllable = false;
				Cursor.visible = true;
				lowerUI.SetActive (false);
				menuBool = true;
			}else{
			//toggle menus
				title.SetActive(false);
				menu.SetActive (false);
				aimTarget.SetActive (true);
				lowerUI.SetActive (true);
			//toggle player components
				player.GetComponentInChildren<Player_Controller>().isControllable = true;
				cam.GetComponent<Player_Camera>().isControllable = true;
				Cursor.visible = false;
				menuBool = false;
			}
		}
	}

    void PlayerBuildMenuRemovePlayerBuildUIs()
    {
        GameObject[] playerBuildUIs = GameObject.FindGameObjectsWithTag("PlayerBuildDisplayPlayerUI");
        foreach (GameObject playerBuildUI in playerBuildUIs)
        {
            CanvasRenderer[] renderers = playerBuildUI.GetComponentsInChildren<CanvasRenderer>();
            foreach (CanvasRenderer renderer in renderers)
            {
                if (renderer.GetAlpha() != 0f)
                {
                    renderer.SetAlpha(0f);
                }
            }
        }
    }

    void RefreshPlayerBuilds()
    {
        GameObject[] playersArray = GameObject.FindGameObjectsWithTag("Player");
        
        string[] seperator = new string[] { " " };

        foreach (GameObject player in playersArray)
        {
            string playerPos = player.transform.name.Split(seperator, System.StringSplitOptions.RemoveEmptyEntries)[0];

            Player_Stats stats = player.GetComponent<Player_Stats>();

            UpdateFirstDamageTalentUI(stats.syncFirstDamageTalentLevel, GameObject.Find(playerPos + "DamageFirstTalentUI").GetComponent<RectTransform>());
            UpdateSecondDamageTalentUI(stats.syncSecondDamageTalentLevel, GameObject.Find(playerPos + "DamageSecondTalentUI").GetComponent<RectTransform>());
            UpdateThirdDamageTalentUI(stats.syncThirdDamageTalentLevel, GameObject.Find(playerPos + "DamageThirdTalentUI").GetComponent<RectTransform>());
            UpdateFirstDefenseTalentUI(stats.syncFirstDefenseTalentLevel, GameObject.Find(playerPos + "DefenseFirstTalentUI").GetComponent<RectTransform>());
            UpdateSecondDefenseTalentUI(stats.syncSecondDefenseTalentLevel, GameObject.Find(playerPos + "DefenseSecondTalentUI").GetComponent<RectTransform>());
            UpdateThirdDefenseTalentUI(stats.syncThirdDefenseTalentLevel, GameObject.Find(playerPos + "DefenseThirdTalentUI").GetComponent<RectTransform>());
            UpdateFirstMobilityTalentUI(stats.syncFirstMobilityTalentLevel, GameObject.Find(playerPos + "MobilityFirstTalentUI").GetComponent<RectTransform>());
            UpdateSecondMobilityTalentUI(stats.syncSecondMobilityTalentLevel, GameObject.Find(playerPos + "MobilitySecondTalentUI").GetComponent<RectTransform>());
            UpdateThirdMobilityTalentUI(stats.syncThirdMobilityTalentLevel, GameObject.Find(playerPos + "MobilityThirdTalentUI").GetComponent<RectTransform>());
            UpdateFirstSupportTalentUI(stats.syncFirstSupportTalentLevel, GameObject.Find(playerPos + "SupportFirstTalentUI").GetComponent<RectTransform>());
            UpdateSecondSupportTalentUI(stats.syncSecondSupportTalentLevel, GameObject.Find(playerPos + "SupportSecondTalentUI").GetComponent<RectTransform>());
            UpdateThirdSupportTalentUI(stats.syncThirdSupportTalentLevel, GameObject.Find(playerPos + "SupportThirdTalentUI").GetComponent<RectTransform>());

            string[] objectNameSplit = player.name.Split(seperator, System.StringSplitOptions.RemoveEmptyEntries);

            GameObject.Find(playerPos + "DisplayNameText").GetComponent<Text>().text = objectNameSplit[1];
            GameObject.Find(playerPos + "LimiterChargeText").GetComponent<Text>().text = stats.charge.ToString();

            //Get Renderers
            CanvasRenderer[] renderers = GameObject.Find(playerPos + "layer").GetComponentsInChildren<CanvasRenderer>();

            if (player.GetComponent<Player_Stats>().isDead)
            {
                foreach (CanvasRenderer renderer in renderers)
                {
                    if (!renderer.gameObject.name.Contains("Respawn"))
                    {
                        renderer.SetAlpha(renderer.GetColor().a * 0.5f);
                    }
                    else 
                    {
                        renderer.SetAlpha(1f);
                        renderer.gameObject.GetComponent<Text>().text = ((int)(stats.respawnTime - (Network.time - stats.timeOfDeath))).ToString();
                    }
                }
            }
            else if (!player.GetComponent<Player_Stats>().isDead)
            {
                foreach (CanvasRenderer renderer in renderers)
                {
                    if (renderer.gameObject.name.Contains("Respawn"))
                    {
                        renderer.SetAlpha(0f);
                    }
                    else if (renderer.GetAlpha() != 1f){
                        renderer.SetAlpha(1f);
                    }
                }
            }
        }
    }

    void UpdateFirstDamageTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(-65f, 18f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(-78f, 20f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(-90f, 23f, 0f);
            Vector3 newScale = new Vector3(6, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateSecondDamageTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(-50f, 50f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if(level == 2)
        {
            Vector3 newPosition = new Vector3(-58f, 58f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(-68f, 68f, 0f);
            Vector3 newScale = new Vector3(6f, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateThirdDamageTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(-19f, 70f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(-20f, 77f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(-23, 90f, 0f);
            Vector3 newScale = new Vector3(6f, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.red;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateFirstDefenseTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(19f, 70f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(20f, 77f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(23, 90f, 0f);
            Vector3 newScale = new Vector3(6f, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateSecondDefenseTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(50f, 50f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(58f, 58f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(68f, 68f, 0f);
            Vector3 newScale = new Vector3(6f, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateThirdDefenseTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(65f, 18f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(78f, 20f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(90f, 23f, 0f);
            Vector3 newScale = new Vector3(6, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.blue;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateFirstMobilityTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(65f, -18f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(78f, -20f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(90f, -23f, 0f);
            Vector3 newScale = new Vector3(6, 6f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateSecondMobilityTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(50f, -50f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(58f, -58f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(68f, -68f, 0f);
            Vector3 newScale = new Vector3(6f, 6f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateThirdMobilityTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(19f, -70f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(20f, -77f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(23, -90f, 0f);
            Vector3 newScale = new Vector3(6f, 6f, 1f);
            UIelement.GetComponent<Image>().color = new Color(0.988f, 0.322f, 1f);
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateFirstSupportTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(-19f, -70f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(-20f, -77f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(-23, -90f, 0f);
            Vector3 newScale = new Vector3(6f, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateSecondSupportTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(-50f, -50f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(-58f, -58f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(-68f, -68f, 0f);
            Vector3 newScale = new Vector3(6f, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

    void UpdateThirdSupportTalentUI(int level, RectTransform UIelement)
    {
        if (level == 1)
        {
            Vector3 newPosition = new Vector3(-65f, -18f, 0f);
            Vector3 newScale = new Vector3(3f, 2f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 2)
        {
            Vector3 newPosition = new Vector3(-78f, -20f, 0f);
            Vector3 newScale = new Vector3(4f, 3f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
        else if (level == 3)
        {
            Vector3 newPosition = new Vector3(-90f, -23f, 0f);
            Vector3 newScale = new Vector3(6, 6f, 1f);
            UIelement.GetComponent<Image>().color = Color.green;
            UIelement.localPosition = newPosition;
            UIelement.localScale = newScale;
        }
    }

}
