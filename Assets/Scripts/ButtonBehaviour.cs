using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonBehaviour : MonoBehaviour {

	Player_Stats stats;
	Player_Menus menu;
	Character_Stats charStats;
    Image leftTalentTextBox;
    Text leftTalentText;

    public GameObject settingsMenu;
    public UserSettings_HG userSettings;
    public GameObject settingsButton;
    public GameObject quitButton;

    Slider mouseSensitivitySlider;
    Toggle yAxisInversionToggle;

    public RectTransform button;
    Button[] allButtonsActive;

    bool isLeftTalentTextBoxOpen = false;

	float UIXPos;
	float UIYPos;
	float xScale;
	float yScale;

	void Start(){
        mouseSensitivitySlider = GameObject.Find("MouseSensitivitySlider").GetComponent<Slider>();
        yAxisInversionToggle = GameObject.Find("InvertYAxisToggle").GetComponent<Toggle>();
        mouseSensitivitySlider.value = userSettings.mouseSensitivity;
        yAxisInversionToggle.isOn = userSettings.invertYAxis;
    }

	public void GetPlayerObject (GameObject player){
		stats = player.GetComponent<Player_Stats> ();
		menu = player.GetComponent<Player_Menus> ();
		charStats = player.GetComponent<Character_Stats> ();
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        settingsButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        settingsButton.SetActive(true);
        quitButton.SetActive(true);
    }

    public void ChangeMouseSensitivity()
    {
        userSettings.mouseSensitivity = mouseSensitivitySlider.value;
    }

    public void ToggleInvertYAxis()
    {
        userSettings.invertYAxis = yAxisInversionToggle.isOn;
    }

    public void GameQuit(){
		Application.Quit();
	}

	public void HighLightedButton(Button button){
		if (isLeftTalentTextBoxOpen == false) {
			menu.leftTalentTextBox.enabled = true;
            menu.leftTalentText.GetComponent<Text> ().enabled = true;
            isLeftTalentTextBoxOpen = true;
		}

		UpdateLeftTalentText (button.name);
	}

	public void UnHighlightedButton(){
		if (isLeftTalentTextBoxOpen == true) {
            menu.leftTalentTextBox.enabled = false;
            menu.leftTalentText.enabled = false;
            menu.leftTalentMouseDiagram.enabled = false;
            menu.leftTalentLeftMouseButton.enabled = false;
            menu.leftTalentRightMouseButton.enabled = false;
            isLeftTalentTextBoxOpen = false;
		}
	}

    public void DisableAllButtons()
    {
        allButtonsActive = GameObject.FindObjectsOfType<Button>();

        foreach (Button button in allButtonsActive)
        {
            button.interactable = false;
        }
    }

    public void ActivateAllDisabledButtons()
    {
        foreach (Button button in allButtonsActive)
        {
            button.interactable = true;
        }
    }

    public void UpdateLeftTalentText(string buttonName){
		if (buttonName == "DamageFirstTalentButton") {
			menu.leftTalentText.text = charStats.firstDamageTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.firstDamageTalentDescription;
			DisplayMouseDiagram(charStats.firstDamageTalentType);
		}
		if (buttonName == "DamageSecondTalentButton") {
			menu.leftTalentText.text = charStats.secondDamageTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.secondDamageTalentDescription;
			DisplayMouseDiagram(charStats.secondDamageTalentType);
		}
		if (buttonName == "DamageThirdTalentButton") {
			menu.leftTalentText.text = charStats.thirdDamageTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.thirdDamageTalentDescription;
			DisplayMouseDiagram(charStats.thirdDamageTalentType);
		}
		if (buttonName == "DefenseFirstTalentButton") {
			menu.leftTalentText.text = charStats.firstDefenseTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.firstDefenseTalentDescription;
			DisplayMouseDiagram(charStats.firstDefenseTalentType);
		}
		if (buttonName== "DefenseSecondTalentButton") {
			menu.leftTalentText.text = charStats.secondDefenseTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.secondDefenseTalentDescription;
			DisplayMouseDiagram(charStats.secondDefenseTalentType);
		}
		if (buttonName == "DefenseThirdTalentButton") {
			menu.leftTalentText.text = charStats.thirdDefenseTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.thirdDefenseTalentDescription;
			DisplayMouseDiagram(charStats.thirdDefenseTalentType);
		}
		if (buttonName == "MobilityFirstTalentButton") {
			menu.leftTalentText.text = charStats.firstMobilityTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.firstMobilityTalentDescription;
			DisplayMouseDiagram(charStats.firstMobilityTalentType);
		}
		if (buttonName == "MobilitySecondTalentButton") {
			menu.leftTalentText.text = charStats.secondMobilityTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.secondMobilityTalentDescription;
			DisplayMouseDiagram(charStats.secondMobilityTalentType);
		}
		if (buttonName == "MobilityThirdTalentButton") {
			menu.leftTalentText.text = charStats.thirdMobilityTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.thirdMobilityTalentDescription;
			DisplayMouseDiagram(charStats.thirdMobilityTalentType);
		}
		if (buttonName == "SupportFirstTalentButton") {
			menu.leftTalentText.text = charStats.firstSupportTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.firstSupportTalentDescription;
			DisplayMouseDiagram(charStats.firstSupportTalentType);
		}
		if (buttonName == "SupportSecondTalentButton") {
			menu.leftTalentText.text = charStats.secondSupportTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.secondSupportTalentDescription;
			DisplayMouseDiagram(charStats.secondSupportTalentType);
		}
		if (buttonName == "SupportThirdTalentButton") {
			menu.leftTalentText.text = charStats.thirdSupportTalentName + System.Environment.NewLine + System.Environment.NewLine +  charStats.thirdSupportTalentDescription;
			DisplayMouseDiagram(charStats.thirdSupportTalentType);
		} 
	}

	void DisplayMouseDiagram(int talentType){
		if (talentType == 0) {
			menu.leftTalentMouseDiagram.enabled = false;
			menu.leftTalentLeftMouseButton.enabled = false;
			menu.leftTalentRightMouseButton.enabled = false;
		}
		if (talentType == 1) {
			menu.leftTalentMouseDiagram.enabled = true;
			menu.leftTalentLeftMouseButton.enabled = true;
		}
		if (talentType == 2) {
			menu.leftTalentMouseDiagram.enabled = true;
			menu.leftTalentRightMouseButton.enabled = true;
		}

	}

	public void IncreaseFirstDamageTalent(){
		if (stats.syncFirstDamageTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseFirstDamageTalentLevel();
		}
	}

	public void IncreaseSecondDamageTalent(){
		if (stats.syncSecondDamageTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseSecondDamageTalentLevel();
		}
	}

	public void IncreaseThirdDamageTalent(){
		if (stats.syncThirdDamageTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseThirdDamageTalentLevel();
		}
	}

	public void IncreaseFirstDefenseTalent(){
		if (stats.syncFirstDefenseTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseFirstDefenseTalentLevel();
		}
	}

	public void IncreaseSecondDefenseTalent(){
        if (stats.syncSecondDefenseTalentLevel < 3 && stats.charge >= 100)
        {
            DisableAllButtons();
            stats.CmdIncreaseSecondDefenseTalentLevel();
        }
	}

	public void IncreaseThirdDefenseTalent(){
        if (stats.syncThirdDefenseTalentLevel < 3 && stats.charge >= 100)
        {
            DisableAllButtons();
            stats.CmdIncreaseThirdDefenseTalentLevel();
        }
	}

	public void IncreaseFirstMobilityTalent(){
		if (stats.syncFirstMobilityTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseFirstMobilityTalentLevel();
		}
	}

	public void IncreaseSecondMobilityTalent(){
		if (stats.syncSecondMobilityTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseSecondMobilityTalentLevel();
		}
	}

	public void IncreaseThirdMobilityTalent(){
		if (stats.syncThirdMobilityTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseThirdMobilityTalentLevel();
		}
	}

	public void IncreaseFirstSupportTalent(){
		if (stats.syncFirstSupportTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseFirstSupportTalentLevel();
		}
	}

	public void IncreaseSecondSupportTalent(){
		if (stats.syncSecondSupportTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseSecondSupportTalentLevel();
		}
	}

	public void IncreaseThirdSupportTalent(){
		if (stats.syncThirdSupportTalentLevel < 3 && stats.charge >= 100) {
            DisableAllButtons();
            stats.CmdIncreaseThirdSupportTalentLevel();
		}
	}


}
